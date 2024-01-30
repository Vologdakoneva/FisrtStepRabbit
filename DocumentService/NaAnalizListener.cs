using DocumentService.Data;
using DocumentService.Entities;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Plain.RabbitMQ;
using static DocumentService.Entities.Docsummary;
using PromedExchange;
using System.Text.Json;
using System.Xml.Linq;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace DocumentService
{
    public class NaAnalizListener : IHostedService
    {
        private readonly ISubscriber subscriber;
        private readonly IServiceScopeFactory serviceScopeFactory;
        private readonly IConfiguration configuration;

        public NaAnalizListener(ISubscriber subscriber, IServiceScopeFactory serviceScopeFactory, IConfiguration configuration) //, IPacientOperator pacientOperator
        {
            this.subscriber = subscriber;
            this.serviceScopeFactory = serviceScopeFactory;
            this.configuration = configuration;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            subscriber.Subscribe(SubscribeAnaliz);
            return Task.CompletedTask;
        }

        private bool SubscribeAnaliz(string message, IDictionary<string, object> dictionary)
        {
            Console.WriteLine(" Сообщение получено " + DateTime.Now,ToString() + "\n");






            Docsummary responsedoc;
            try
            {
                responsedoc = JsonConvert.DeserializeObject<Docsummary>(message);
            }
            catch (Exception)
            {

                Console.WriteLine(" Сообщение ошибка данных " + "\n");
                return true;
            }
            using (var scope = serviceScopeFactory.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<DocumentDbContext>();

                DocAnaliz? docAnaliz = dbContext.DocAnaliz.Where(p => p.DocLink == responsedoc.docAnaliz.DocLink).FirstOrDefault();
                if (docAnaliz == null)
                {
                    docAnaliz = responsedoc.docAnaliz;
                    docAnaliz.Items = JsonConvert.SerializeObject(responsedoc.Items);
                    dbContext.DocAnaliz.Add(docAnaliz);
                    dbContext.SaveChanges();
                    docAnaliz = dbContext.DocAnaliz.Where(p => p.DocLink == responsedoc.docAnaliz.DocLink).FirstOrDefault();
                }
                else
                {
                    var entityProperties = docAnaliz.GetType().GetProperties();
                    dbContext.Entry(docAnaliz).State = EntityState.Modified;
                    dbContext.DocAnaliz.Attach(docAnaliz);

                    foreach (var ep in entityProperties)
                    {
                        if (ep.Name != "IDALL" && ep.Name != "DataCreatePerson")
                        {
                            //dbContext.Entry(docAnaliz.DateChangePerson = DateTime.Now;).Property(ep.Name).IsModified = true;
                            dbContext.Entry(docAnaliz).Property(ep.Name).CurrentValue = dbContext.Entry(responsedoc.docAnaliz).Property(ep.Name).CurrentValue;
                        }
                    }                        //response.IDALL = person.IDALL;
                                             //person = response;
                                             //dbContext.Entry(person).CurrentValues.SetValues(response);
                                             //dbContext.Entry(person).State = EntityState.Modified;
                    docAnaliz.DataChange = DateTime.Now;
                    dbContext.SaveChanges();

                }

                int rrr = 0;
                if (docAnaliz.Fio== "РЯБЕВА СВЕТЛАНА НИКОЛАЕВНА" && rrr==0)
                {
                    rrr = 10;
                }

                List<DocError> docErrors = new List<DocError>();
                string urlPacientService = configuration.GetConnectionString("PacientService");
                string Respose = "";
                try
                {
                    Respose = GetAsync(urlPacientService + "/" + docAnaliz.Fiokey);
                }
                catch (Exception)
                {
                    Respose = "";
                }
                Person person = JsonConvert.DeserializeObject<Person>(Respose);

                if (person == null || person.FamilyPerson == "")
                {
                    docErrors.Add(new DocError() { ErrorSource = "1С", ErrorText = "Пациент " + docAnaliz.Fio + " не передавался. в 1С отредактируйе его" });
                    docAnaliz.Errors = JsonConvert.SerializeObject(docErrors);
                    dbContext.SaveChanges();
                    return true;
                }
                docAnaliz.IdFio = person.IdPromedPerson;
                try
                {
                    Respose = GetAsync(urlPacientService + "/" + docAnaliz.FioDoctorkey);
                }
                catch (Exception)
                {
                    Respose = "";
                }
                person = JsonConvert.DeserializeObject<Person>(Respose);
                if (person == null || person.FamilyPerson == "")
                {
                    docErrors.Add(new DocError() { ErrorSource = "1С", ErrorText = "Врач " + docAnaliz.FioDoctor + "не передавался. в 1С отредактируйе его" });
                    docAnaliz.Errors = JsonConvert.SerializeObject(docErrors);
                    dbContext.SaveChanges();
                    return true;
                }
                docAnaliz.IdDoctor = person.IdPromedPerson;
                dbContext.SaveChanges();
                // debug only
                //  return true;

                // Опредяем врача
                Int64 MedWorker_id = 0;
                Int64 LpuSection_id = 0;
                Int64 MedStaffFact_id = 0;
                string response = ""; JsonElement element;
                PromedExchange.Promed promed = new PromedExchange.Promed(false);
                response = promed.SendGet("/api/MedWorker?Person_id=" + docAnaliz.IdDoctor);
                if (promed.GetErrorCode())
                {
                    element = promed.GetData();
                    if (!(element is object))
                    {
                        response = promed.SendPost("/api/MedWorker", "Person_id=" + docAnaliz.IdDoctor);
                        JsonElement medData;
                        if (promed.GetErrorCode())
                        {
                            medData = promed.GetData();
                            MedWorker_id = Convert.ToInt64(medData.GetProperty("MedWorker_id").GetString());
                        }
                    }
                    ;
                    if (MedWorker_id<=0 && (element is object))
                    {
                        try
                        {
                            MedWorker_id = Convert.ToInt64(element.GetProperty("MedWorker_id").GetString());
                        }
                        catch (Exception)
                        {
                        }
                    }
                    if (MedWorker_id <= 0)
                    {
                        docErrors.Add(new DocError() { ErrorSource = "Промед", ErrorText = "Врач " + docAnaliz.FioDoctor + "  не определен" });
                        docAnaliz.Errors = JsonConvert.SerializeObject(docErrors);
                        dbContext.SaveChanges();
                        return true;
                    }
                    if (docAnaliz.UetHead == null || docAnaliz.UetHead == "")
                    {
                        docErrors.Add(new DocError() { ErrorSource = "1c", ErrorText = "Не указан код УЕТ анализа " });
                        docAnaliz.Errors = JsonConvert.SerializeObject(docErrors);
                        dbContext.SaveChanges();
                        return true;
                    }
                    response = promed.SendGet("/api/Lpu/LpuSectionListByMO" + "?Lpu_id=" + 12600044);
                    if (promed.GetErrorCode())
                    {
                        element = promed.GetData();
                        LpuSection_id = Convert.ToInt64(element[0].GetProperty("LpuSection_id").GetString());
                    }
                    // Получение мест работы медицинского работника 
                    response = promed.SendGet("/api/WorkPlace" +
                                              "?MedWorker_id=" + MedWorker_id
                                              );
                    if (promed.GetErrorCode())
                    {
                        element = promed.GetData();
                        LpuSection_id = Convert.ToInt64(element[0].GetProperty("LpuSection_id").GetString());
                        response = promed.SendGet("/api/MedStaffFactByMedPersonal" +
                                              "?MedPersonal_id=" + MedWorker_id +
                                              "&LpuSection_id=" + LpuSection_id);
                        element = promed.GetData();
                        MedStaffFact_id = Convert.ToInt64(element[0].GetProperty("MedStaffFact_id").GetString());
                    }
                    if (MedStaffFact_id == 0 || LpuSection_id == 0)
                    {
                        docErrors.Add(new DocError() { ErrorSource = "Промед", ErrorText = "Не указано место работы врача " + docAnaliz.FioDoctor + " не определено" });
                        docAnaliz.Errors = JsonConvert.SerializeObject(docErrors);
                        dbContext.SaveChanges();
                        return true;
                    }
                    //Создание ТАП с первым посещением для поликлинического случая
                    Int64 EvnPLBase_id = 0;
                    Int64 EvnVizitPL_id = 0;   //идентификатор посещения;
                    response = promed.SendGet("/api/EvnPLBase?Person_id=" + docAnaliz.IdFio);

                    if (promed.GetErrorCode())
                    {

                        element = promed.GetData();
                        if (element.GetArrayLength() > 0)
                        {
                            EvnVizitPL_id = Convert.ToInt64(element[0].GetProperty("EvnDirection_id").GetString());   //идентификатор посещения;
                            EvnPLBase_id = Convert.ToInt64(element[0].GetProperty("EvnPLBase_id").GetString());    //идентификатор
                        }
                    }
                    string UslugaComplex_id_Head = "";
                    //if (EvnPLBase_id == 0)
                    //{
                        response = promed.SendGet("/api/RefbookUslugaComplex?UslugaComplex_Code=" + docAnaliz.UetHead ); //+"&LpuSection_id=" + LpuSection_id
                        if (promed.GetErrorCode())
                        {
                            element = promed.GetData();
                            if (element.GetArrayLength() > 0)
                            {
                            for (int i = 0; i < element.GetArrayLength(); i++)
                            {
                                if (element[i].GetProperty("UslugaComplex_Code").GetString() == docAnaliz.UetHead && element[i].GetProperty("UslugaComplex_pid").GetString() != null)
                                    UslugaComplex_id_Head = element[i].GetProperty("UslugaComplex_id").GetString();
                            }
                               
                            }
                        }
                    //}
                    int Analyzer_id = 0;
                    string UslugaComplexMedService_ResIdtmp = "";
                    string UslugaComplexMedService_ResId = "";
                    string UslugaComplex_ResCode = "";
                    string UslugaComplex_ResName = "";
                    int UslugaComplex_id = 0;
                    int UslugaComplexMedService_id = 0;
                    string UslugaComplex_Code = "";
                    string KodelistID = "";
                    string KodelistIDComplex = "";

                    response = promed.SendGet("api/UslugaComplexMedService?MedService_id=" + 13090
                                               + "&UslugaComplex_Code=" + docAnaliz.UetHead + "&ResponseFull=1");
                    if (promed.GetErrorCode())
                    {

                        DocItems[]? itemsdoc = JsonConvert.DeserializeObject<DocItems[]>(docAnaliz.Items);

                        if (itemsdoc == null || itemsdoc.Count() ==0)
                        {
                            docErrors.Add(new DocError() { ErrorSource = "1с", ErrorText = "Анализов не определено" });
                            docAnaliz.Errors = JsonConvert.SerializeObject(docErrors);
                            dbContext.SaveChanges();
                            return true;

                        }

                        element = promed.GetData();
                        if (element.GetArrayLength()==0)
                        {
                            docErrors.Add(new DocError() { ErrorSource = "промед", ErrorText = "Не найдены коды УЕТ" });
                            docAnaliz.Errors = JsonConvert.SerializeObject(docErrors);
                            dbContext.SaveChanges();
                            return true;

                        }
                        for (int i = 0; i < itemsdoc.Count(); i++)
                        {
                            if (itemsdoc[i].uet == "" || itemsdoc[i].uet == null)
                            {
                                docErrors.Add(new DocError() { ErrorSource = "1c", ErrorText = "нет УЕТ " + itemsdoc[i].AnalizText });
                                docAnaliz.Errors = JsonConvert.SerializeObject(docErrors);
                                dbContext.SaveChanges();
                                return true;
                            }

                        }
                        

                        response = promed.SendGet("/api/UslugaComplexMedService/?MedService_id=" + 13090 //
                                                   + "&ResponseFull=1"); //+ "&UslugaComplex_Code=" + docAnaliz.UetHead
                        if (promed.GetErrorCode()) {
                            element = promed.GetData();

                                for (int i = 0; i < element.GetArrayLength(); i++)
                                {
                                    UslugaComplex_ResCode = element[i].GetProperty("UslugaComplex_ResCode").GetString();
                                if (docAnaliz.UetHead == UslugaComplex_ResCode) {
                                    UslugaComplexMedService_ResId = element[i].GetProperty("UslugaComplexMedService_ResId").GetString();
                                    
                                    JsonElement data = element[i].GetProperty("UslugaComplexMedService_TestList"); 

                                    for (int j = 0; j < data.GetArrayLength(); j++)
                                    {
                                        for (int ii = 0; ii < itemsdoc.Count(); ii++)
                                        {
                                            if (data[j].GetProperty("UslugaComplex_Code").GetString() == itemsdoc[ii].uet)
                                            {
                                                itemsdoc[ii].kod = data[j].GetProperty("UslugaComplex_id").GetString();
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        docAnaliz.Items = JsonConvert.SerializeObject(itemsdoc);
                        dbContext.Entry(docAnaliz).State = EntityState.Modified;
                        dbContext.DocAnaliz.Attach(docAnaliz);
                        dbContext.SaveChanges();


                    }
                    if (UslugaComplexMedService_ResId=="")
                    {
                        docErrors.Add(new DocError() { ErrorSource = "промед", ErrorText = " UslugaComplexMedService_ResId не найден " });
                        docAnaliz.Errors = JsonConvert.SerializeObject(docErrors);
                        dbContext.SaveChanges();
                        return true;

                    }
                    if (EvnPLBase_id == 0) {

                        string vidoplID = "";
                        response = promed.SendGet("/api/Refbook?Refbook_Code=" + "1.2.643.5.1.13.2.1.1.104" + "&Name=" + "ОМС");
                        JsonElement vidplElement = promed.GetData();
                        vidoplID = vidplElement[0].GetProperty("id").GetString();

                        response = promed.SendPost("/api/EvnPLBase",
                               "Person_id=" + docAnaliz.IdFio + // идентификатор человека
                               "&EvnPL_NumCard=" + docAnaliz.NomDoc + //№ талона. Уникальный в рамках МО.
                               "&EvnPL_IsFinish=" + "0" +   //  Признак законченности случая. Возможные значения: 0 и 1, – где 0 – нет, 1 – да
                               "&Evn_setDT=" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + //Дата и время посещения ГГГГ–ММ–ДД чч:мм:сс;
                               "&EvnVizitPL_Time=" + 45 +
                               "&LpuSection_id=" + LpuSection_id + //16654 + //LpuSection_id + //"2635" LpuSection_id + //идентификатор отделения МО  dbo.LpuSection
                               "&MedStaffFact_id=" + MedStaffFact_id + //Специальность врача: идентификатор справочника медицинских работников MedStaffFact_id   MedWorker_id
                               "&TreatmentClass_id=" + "2" + //Вид обращения  dbo.TreatmentClass
                               "&ServiceType_id=" + 1 + //Место обслуживания dbo.ServiceType Поликлинника
                               "&VizitType_id=" + 583 + //цель посещения  dbo.VizitType Прочие профилактические посещения (не
                               "&PayType_id=" + vidoplID + //Тип оплаты
                               "&Mes_id=" + 1 + //МЭС (передается только для первого посещения) dbo.MesLevel
                               "&UslugaComplex_uid=" + 4550001 + //UslugaComplex_id_Head + //"4550001"+ //код посещения; Обязательно если вид оплаты ОМС
                               "&Diag_id=" + 2 + //Основной диагноз (В ТЗ поле не обязательное, на форме обязательное)
                               "&EvnDirection_setDate=" + docAnaliz.Datadoc.ToString("yyyy-MM-dd") + //
                               "&MedicalCareKind_id=" + 5 //Идентификатор вида медицинской помощи
                      );
                        if (promed.GetErrorCode())
                        {
                            JsonElement kodeElement = promed.GetData();
                            EvnVizitPL_id = Convert.ToInt32(kodeElement[0].GetProperty("EvnVizitPL_id").GetString());   //идентификатор посещения;
                            EvnPLBase_id = Convert.ToInt32(kodeElement[0].GetProperty("EvnPLBase_id").GetString()); 	//идентификатор

                        }
                    }

                    List<string> kodelist = new List<string>();
                    DocItems[]? items = JsonConvert.DeserializeObject<DocItems[]>(docAnaliz.Items);
                    
                    for (int i = 0; i < items.Count(); i++)
                    {
                        kodelist.Add(items[i].kod);
                        if (KodelistIDComplex != "")
                            KodelistIDComplex = KodelistIDComplex + ",";
                        KodelistIDComplex = KodelistIDComplex + items[i].kod;

                    }

                    string KodelistArrayJSON = JsonConvert.SerializeObject(kodelist);

                    Int64 EvnDirection_id = 0;
                    string Evn_id = ""; string EvnQueue_id = ""; string EvnPrescr_id = "";

                    response = promed.SendGet("/api/EvnDirection" +
                                "?Person_id=" + docAnaliz.IdFio + //EvnVizitPL_id  EvnPLBase_id
                               "&EvnDirection_Num=" + docAnaliz.NomDoc +
                               "&LpuSection_id=" + LpuSection_id +
                               "&Lpu_sid=" + 12600044 +
                               "&EvnDirection_setDate=" + docAnaliz.Datadoc.ToString("yyyy-MM-dd") 
                                );
                    if (promed.GetErrorCode())
                    {
                        JsonElement Napre = promed.GetData();
                        if (Napre.GetArrayLength() == 1) {
                            EvnDirection_id = Convert.ToInt64( Napre[0].GetProperty("EvnDirection_id").GetString()); // идентификатор направления
                            Evn_id = Napre[0].GetProperty("Evn_id").GetString();                   // идентификатор события;
                            EvnQueue_id = Napre[0].GetProperty("EvnQueue_id").GetString();         // идентификатор постановки в очередь
                            EvnPrescr_id = Napre[0].GetProperty("EvnPrescr_id").GetString();       // идентификатор назначения
                        }
                    }
                    if (EvnDirection_id == 0) {

                        response = promed.SendPost("/api/EvnDirection",
                                 "Evn_pid=" + EvnPLBase_id + //EvnVizitPL_id  EvnPLBase_id
                               "&Person_id=" + docAnaliz.IdFio +
                               "&EvnDirection_Num=" + docAnaliz.NomDoc +
                               "&LpuSection_id=" + LpuSection_id +
                               "&Lpu_sid=" + 12600044 +
                               "&EvnDirection_setDate=" + docAnaliz.Datadoc.ToString("yyyy-MM-dd") +
                               "&PayType_id=" + "190" +  // vidoplID
                               "&DirType_id=" + "10" +  // На исследование
                               "&Diag_id=" + "null" + // 2??
                               "&MedPersonal_id=" + MedWorker_id +  // направивший врач
                               "&MedStaffFact_id=" + MedStaffFact_id +  // Место работы направившего
                               "&Lpu_did=" + 12600044 +
                               "&MedPersonal_did=" + 533 +  //врач, к кому направили
                               "&LpuSectionProfile_id=" + 402275 + //402275 + // Идентификатор профиля  402266   профиль, куда направили
                               "&PrescriptionType_id=" + 11 +  // Тип назначения 11- лабораторная диагностика,
                               "&EvnPrescr_IsCito=" + 0 +     // Признак "Cito" (0 – Нет, 1 – Да). Обязательный (Срочно/нет) ??
                               "&UslugaComplexMedService_ResId=" + UslugaComplexMedService_ResId + // ??идентификатор исследования
                                                                                                   //"&UslugaComplexMedService_id=" + UslugaComplexMedService_id + // идентификатор теста
                               "&TestList=" + KodelistArrayJSON + // KodelistArrayJSON список тестов исследования; если массив не заполнен, то в
                                                                  //  направлении должны быть назначены все тесты исследования
                               "&LpuUnitType_id=" + 0 //2  // ??  Условия оказания медицинской помощи. Поликлинника

                      );

                        if (promed.GetErrorCode())
                        {

                            JsonElement Napr = promed.GetData();
                            EvnDirection_id = Convert.ToInt64(Napr.GetProperty("EvnDirection_id").GetString()); // идентификатор направления
                            Evn_id = Napr.GetProperty("Evn_id").GetString();                   // идентификатор события;
                            EvnQueue_id = Napr.GetProperty("EvnQueue_id").GetString();         // идентификатор постановки в очередь
                            EvnPrescr_id = Napr.GetProperty("EvnPrescr_id").GetString();       // идентификатор назначения

                            response = promed.SendGet("/api/EvnDirection?EvnDirection_id=" + EvnDirection_id);


                            if (promed.GetErrorCode())
                            {
                                Napr = promed.GetData();
                                if (Napr.GetArrayLength() > 0)
                                {
                                    EvnDirection_id = Convert.ToInt64(Napr[0].GetProperty("EvnDirection_id").GetString()); // идентификатор направления
                                    Evn_id = Napr[0].GetProperty("Evn_id").GetString();                   // идентификатор события;
                                    EvnQueue_id = Napr[0].GetProperty("EvnQueue_id").GetString();         // идентификатор постановки в очередь
                                    EvnPrescr_id = Napr[0].GetProperty("EvnPrescr_id").GetString();      // идентификатор назначения


                                }
                            }





                        }
                        else
                           ;
                }
                    // Добавление информации о факте взятия пробы по направлению
                    response = promed.SendPost("/api/EvnLabSample",
                                   "EvnDirection_id=" + EvnDirection_id +
                                 "&EvnLabSample_setDT=" + docAnaliz.Datadoc.AddMinutes(5).ToString("yyyy-MM-dd hh:mm:ss")  +
                                 "&EvnLabSample_StudyDT=" + docAnaliz.Datadoc.AddMinutes(5).ToString("yyyy-MM-dd hh:mm:ss")  +
                                 "&EvnLabSample_AnalyzerDate=" + docAnaliz.Databiomaterial.AddMinutes(6).ToString("yyyy-MM-dd hh:mm:ss") +
                                 "&EvnLabSample_AnalyzerDate=" + docAnaliz.Databiomaterial.AddMinutes(6).ToString("yyyy-MM-dd hh:mm:ss") +
                                 "&UslugaComplexList=" + KodelistIDComplex + //UslugaComplex_id_Head + /// KodelistIDComplex KodelistУЕТ  KodelistУЕТHead
                                                                             //"&Lpu_did=12600044"
                                 "&LpuSection_did=16654" + //LpuSection_id+  //id отделения, 16654
                                 "&MedService_did=13090" +  // id служба 13090
                                                            //"&EvnLabSample_AnalyzerDate="+ Формат(ВыборкаДетальныеЗаписи.Дата, "ДФ=yyyy-MM-dd")+ " " + Формат(ТекущаяДата(),"ДФ=hh:mm:ss") +
                                                            //"&MedService_did=" + 16654 + //MedStaffFact_id + 45593   //место работы врача, взявшего пробу для Кочневой 350101000008445
                                 "&MedStaffFact_did=" + 350101000008445 + ///KodelistУЕТ  KodelistУЕТHead
                                 "&MedPersonal_did=" + 533 //533 //+ MedWorker_id  27309  //врач, взявший пробу, кочнева мед персонал 533


                                 );
                    string messageuet = "";
                    if (promed.GetErrorCode()) {
                        JsonElement Probe = promed.GetData();
                        Int64 EvnLabSample_id = Convert.ToInt64(Probe.GetProperty("EvnLabSample_id").GetString()); // идентификатор направления
                        List < ItemPromed > promeds = new List<ItemPromed>();
                                    for (int i = 0; i < items.Length; i++)
                                    {
                                        if (items.ElementAt(i).kod != "")
                                        {
                                            promeds.Add(new ItemPromed()
                                            {
                                                UslugaComplex_id = items.ElementAt(i).kod,
                                                UslugaTest_ResultValue = items.ElementAt(i).result,
                                                UslugaTest_setDT = docAnaliz.Datadoc.AddMinutes(10).ToString("yyyy-MM-dd hh:mm:ss")

                                            });
                                        }
                                        else { messageuet = messageuet + "нет УЕТ " + items.ElementAt(i).AnalizText; }
                                    }

                        if (messageuet!="")
                        {
                            docErrors.Add(new DocError() { ErrorSource = "промед", ErrorText = messageuet });
                            docAnaliz.Errors = JsonConvert.SerializeObject(docErrors);
                            dbContext.SaveChanges();
                            return true; 
                        }

                        response = promed.SendPost("/api/UslugaTestAll",
                                     "EvnLabSample_id=" + EvnLabSample_id +  //идентификатор пробы
                                     "&UslugaTest_deleted=0" +
                                     "&TestList=" + JsonConvert.SerializeObject(promeds) +
                                     //"&MedStaffFact_id=" + 350101000008445 + //MedStaffFact_id + //45593  место работы врача, выполнившего тест
                                     "&Lpu_id=" + 12600044 + //45593
                                                             //"&EvnDirection_Num=" + Номер+
                                                             //"&EvnDirection_setDate=" + Формат(ВыборкаДетальныеЗаписи.Дата, "ДФ=yyyy-MM-dd") +
                                      "&PayType_id=" + "190" +  // vidoplID
                                                                //"&DirType_id=" + 10 +  // На исследование
                                                                //"&Diag_id=" + 1 + //??
                                      "&Lpu_id=" + 12600044
                                       //"&UslugaTest_setDT=" + Формат(ВыборкаДетальныеЗаписи.Дата , "ДФ=yyyy-MM-dd") + //" " + Формат(ТекущаяДата(),"ДФ=hh:mm:ss")+
                                       //"&MedStaffFact_id=" + 45593    //MedStaffFact_id +  // Место работы направившего
                                       //"&Lpu_did=" + 12600044 +
                                       //"&LpuSectionProfile_id=" + 402266 + // Идентификатор профиля
                                       //"&PrescriptionType_id=" + 11 +  // Тип назначения 11- лабораторная диагностика,
                                       //"&EvnPrescr_IsCito=" + 0 +     // Признак "Cito" (0 – Нет, 1 – Да). Обязательный (Срочно/нет) ??
                                       //"&UslugaComplexMedService_ResId=" + UslugaComplexMedService_ResId + // ?? идентификатор теста
                                       //"&LpuUnitType_id=" + 2  // ??  Условия оказания медицинской помощи.  поликлинника
                                       );

                        if (promed.GetErrorCode())
                        {
                            docErrors.Add(new DocError() { ErrorSource = "промед", ErrorText = "Успешно." });
                            docAnaliz.Errors = JsonConvert.SerializeObject(docErrors);
                            docAnaliz.successfully = true;
                            dbContext.SaveChanges();
                            
                        }
                    }
                    }

                

            }
            Console.WriteLine(" Сообщение обработано  " + "\n");

            return true;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        public string GetAsync(string uri)
        {
            string responseString = "";
            using (var client = new HttpClient())
            {
                var response = client.GetAsync(uri).Result;
                if (response.IsSuccessStatusCode)
                {
                    var responseContent = response.Content;
                    // by calling .Result you are synchronously reading the result
                    responseString = responseContent.ReadAsStringAsync().Result;
                    Console.WriteLine(responseString);
                }
            }
            return responseString;
        }
    }
}
