using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace SEPA.Pain_008
{

    [XmlRoot(ElementName = "InitgPty")]
    public class InitiatingParty
    {

        [XmlElement(ElementName = "Nm")]
        public string Name { get; set; }
    }

    /// <summary>
    /// Kenndaten zur gemeinsamen Nutzung für alle Transaktionen innerhalb der SEPA Nachricht
    /// </summary>
    [XmlRoot(ElementName = "GrpHdr")]
    public class GroupHeader
    {
        /// <summary>
        /// Maximal 35 Zeichen
        /// 
        /// Referenz des Dateierstellers zwecks eindeutiger Identifizierung der SEPA Datei.
        /// </summary>
        [XmlElement(ElementName = "MsgId", Order = 0)]
        public string MessageIdentification { get; set; } = DateTime.Now.ToFileTimeUtc().ToString();

        [XmlElement(ElementName = "CreDtTm", Order = 1)]
        public string CreDtTm = DateTime.Now.ToString("yyyy-MM-dd'T'HH:mm:ss.fffffffZ");

        /// <summary>
        /// ISO Date Time
        /// 
        /// Datum und Zeit wann die SEPA Datei erzeugt wurde
        /// </summary>
        [XmlIgnore]
        public DateTime CreationDateTime
        {
            get
            {
                return DateTime.Parse(CreDtTm);
            }
            set
            {
                CreDtTm = value.ToString("yyyy-MM-dd'T'HH:mm:ss.fffffffZ");
            }
        }



        /// <summary>
        /// Maximal 15 nummerische Stellen
        /// 
        /// Anzahl der einzelnen Transaktionen innerhalb der ganzen SEPA Nachricht
        /// </summary>
        [XmlElement(ElementName = "NbOfTxs", Order = 2)]
        public int NumberOfTransactions { get; set; }

        /// <summary>
        /// Summe aller Einzeltransaktionen in der gesamten SEPA Nachricht
        /// </summary>
        [XmlElement(ElementName = "CtrlSum", Order = 3)]
        public double ControlSum { get; set; }

        [XmlIgnore]
        private InitiatingParty _InitiatingParty;
        /// <summary>
        /// Benennung Auftraggeber bei SEPA Überweisung oder Einreicher bei SEPA Lastschrift
        /// </summary>
        [XmlElement(ElementName = "InitgPty", Order = 4)]
        public InitiatingParty InitiatingParty
        {
            get
            {
                _InitiatingParty = _InitiatingParty ?? new InitiatingParty();
                return _InitiatingParty;
            }
            set
            {
                _InitiatingParty = value;
            }
        }
    }

    [XmlRoot(ElementName = "SvcLvl")]
    public class ServiceLevel
    {

        [XmlElement(ElementName = "Cd")]
        public string Code { get; set; } = "SEPA";
    }

    [XmlRoot(ElementName = "LclInstrm")]
    public class LclInstrm
    {

        [XmlElement(ElementName = "Cd")]
        public string Cd { get; set; } = "CORE";
    }

    [XmlRoot(ElementName = "PmtTpInf")]
    public class PaymentTypeInformation
    {
        [XmlIgnore]
        private ServiceLevel _ServiceLevel;
        /// <summary>
        /// 
        /// </summary>
        [XmlElement(ElementName = "SvcLvl")]
        public ServiceLevel ServiceLevel
        {
            get
            {
                _ServiceLevel = _ServiceLevel ?? new ServiceLevel();
                return _ServiceLevel;
            }
            set
            {
                _ServiceLevel = value;
            }
        }

        [XmlIgnore]
        private LclInstrm _LclInstrm;
        [XmlElement(ElementName = "LclInstrm")]
        public LclInstrm LclInstrm
        {
            get
            {
                _LclInstrm = _LclInstrm ?? new LclInstrm();
                return _LclInstrm;
            }
            set
            {
                _LclInstrm = value;
            }
        }

        /// <summary>
        /// Datenattribut gemäß EPC SEPA Regelwerk: AT-21 SEPA Transaction Type Mögliche Ausprägungen:
        /// FRST(SEPA Erstlastschrift)
        /// RCUR(SEPA Folgelastschrift)
        /// OOFF(SEPA Einmal Lastschrift)
        /// FNAL(Letzte SEPA Lastschrift)
        /// </summary>
        public enum SequenceTypeEnum
        {
            FRST,
            RCUR,
            OOFF,
            FNAL
        }

        /// <summary>
        /// Datenattribut gemäß EPC SEPA Regelwerk: AT-21 SEPA Transaction Type Mögliche Ausprägungen:
        /// FRST(SEPA Erstlastschrift)
        /// RCUR(SEPA Folgelastschrift)
        /// OOFF(SEPA Einmal Lastschrift)
        /// FNAL(Letzte SEPA Lastschrift)
        /// </summary>
        [XmlElement(ElementName = "SeqTp")]
        public SequenceTypeEnum SequenceType { get; set; }



    }

    /// <summary>
    /// SEPA Zahlungsempfänger (SEPA Gläubiger)
    /// </summary>
    [XmlRoot(ElementName = "Cdtr")]
    public class Creditor
    {
        /// <summary>
        /// Maximal 70 Zeichen
        /// Datenattribut gemäß EPC SEPA Regelwerk: AT-03 Name des Zahlungsempfängers
        /// </summary>
        [XmlElement(ElementName = "Nm")]
        public string Name { get; set; }
    }

    /// <summary>
    /// Eindeutige Identifizierung einer Organisation oder Person
    /// </summary>
    [XmlRoot(ElementName = "Id")]
    public class Id
    {
        /// <summary>
        /// Maximal 34 Zeichen 
        /// International Bank Account Number Datenattribut gemäß EPC SEPA Regelwerk: AT-04 IBAN Kontos des Zahlungsempfängers
        /// </summary>
        [XmlElement(ElementName = "IBAN")]
        public string IBAN { get; set; }

        [XmlIgnore]
        private PrivateIdentification _PrivateIdentification;
        /// <summary>
        /// Eindeutiger Code für Eine Identifizierung
        /// </summary>
        [XmlElement(ElementName = "PrvtId")]
        public PrivateIdentification PrivateIdentification
        {
            get
            {
                if (String.IsNullOrWhiteSpace(IBAN))
                {
                    _PrivateIdentification = _PrivateIdentification ?? new PrivateIdentification();
                }

                return _PrivateIdentification;
            }
            set
            {
                _PrivateIdentification = value;
            }
        }
    }

    /// <summary>
    /// Konto Zahlungsempfänger
    /// </summary>
    [XmlRoot(ElementName = "CdtrAcct")]
    public class CreditorAccount
    {
        [XmlIgnore]
        private Id _Id;
        /// <summary>
        /// Eindeutige Identifizierung einer Organisation oder Person
        /// </summary>
        [XmlElement(ElementName = "Id")]
        public Id Id
        {
            get
            {
                _Id = _Id ?? new Id(); return _Id;
            }
            set
            {
                _Id = value;
            }
        }
    }

    [XmlRoot(ElementName = "FinInstnId")]
    public class FinancialInstitutionIdentification
    {

        [XmlElement(ElementName = "BIC")]
        public string BIC { get; set; }
    }

    /// <summary>
    /// Bank des Zahlungsempfängers
    /// </summary>
    [XmlRoot(ElementName = "CdtrAgt")]
    public class CreditorAgent
    {
        [XmlIgnore]
        private FinancialInstitutionIdentification _FinancialInstitutionIdentification;
        /// <summary>
        /// Eindeutige ID einer Bank (BIC)
        /// </summary>
        [XmlElement(ElementName = "FinInstnId")]
        public FinancialInstitutionIdentification FinancialInstitutionIdentification
        {
            get
            {
                _FinancialInstitutionIdentification = _FinancialInstitutionIdentification ?? new FinancialInstitutionIdentification();
                return _FinancialInstitutionIdentification;
            }
            set
            {
                _FinancialInstitutionIdentification = value;
            }
        }
    }

    [XmlRoot(ElementName = "SchmeNm")]
    public class SchmeNm
    {

        [XmlElement(ElementName = "Prtry")]
        public string Prtry { get; set; } = "SEPA";
    }

    /// <summary>
    /// Personen Identifikation, die keiner definierten Identifizierung entspricht
    /// </summary>
    [XmlRoot(ElementName = "Othr")]
    public class OtherIdentification
    {

        [XmlElement(ElementName = "Id")]
        public string Id { get; set; }

        [XmlElement(ElementName = "SchmeNm")]
        public SchmeNm SchmeNm { get; set; } = new SchmeNm();
    }

    /// <summary>
    /// Eindeutiger Code für Eine Identifizierung
    /// </summary>
    [XmlRoot(ElementName = "PrvtId")]
    public class PrivateIdentification
    {
        [XmlIgnore]
        private OtherIdentification _OtherIdentification;
        /// <summary>
        /// Personen Identifikation, die keiner definierten Identifizierung entspricht
        /// </summary>
        [XmlElement(ElementName = "Othr")]
        public OtherIdentification OtherIdentification
        {
            get
            {
                _OtherIdentification = _OtherIdentification ?? new OtherIdentification();
                return _OtherIdentification;
            }
            set
            {
                _OtherIdentification = value;
            }
        }
    }

    [XmlRoot(ElementName = "CdtrSchmeId")]
    public class CreditorSchemeIdentification
    {
        [XmlIgnore]
        private Id _Id;
        [XmlElement(ElementName = "Id")]
        public Id Id
        {
            get
            {
                _Id = _Id ?? new Id();
                return _Id;
            }
            set
            {
                _Id = value;
            }
        }
    }

    [XmlRoot(ElementName = "PmtId")]
    public class PaymentIdentification
    {
        /// <summary>
        /// Eindeutige Referenz des SEPA Lastschrifteinreichers, wird wird durch die gesamte Transaktionskette bis zum Zahler weiter geleitet. Datenattribut gemäß EPC SEPA Regelwerk: SEPA Referenz für den Zahlungspflichtigen
        /// </summary>
        [XmlElement(ElementName = "EndToEndId")]
        public string EndToEndId { get; set; } = "NOTPROVIDED"; /*DateTime.Now.ToFileTimeUtc().ToString();*/
    }

    /// <summary>
    /// Zahlungsbetrag Datenattribut gemäß EPC SEPA Regelwerk: AT-06 Einzugsbetrag SEPA Lastschrift
    /// </summary>
    [XmlRoot(ElementName = "InstdAmt")]
    public class InstructedAmount
    {

        [XmlAttribute(AttributeName = "Ccy")]
        public string Ccy { get; set; } = "EUR";

        [XmlText]
        public double instructedAmount { get; set; }
    }

    /// <summary>
    /// Creditor Identifier auf dem Original SEPA Mandat
    /// </summary>
    [XmlRoot(ElementName = "OrgnlCdtrSchmeId")]
    public class OriginalCreditorSchemeIdentification
    {

        [XmlElement(ElementName = "Nm")]
        public string Name { get; set; }

        [XmlIgnore]
        private Id _Id;
        [XmlElement(ElementName = "Id")]
        public Id Id
        {
            get
            {
                _Id = _Id ?? new Id();
                return _Id;
            }
            set
            {
                _Id = value;
            }
        }
    }

    /// <summary>
    /// Details SEPA Mandatsänderung Datenattribut gemäß EPC SEPA Regelwerk: AT-24 Grund für SEPA Mandatsänderung Pflichtfeld, wenn Amendment Indicator = TRUE
    /// </summary>
    [XmlRoot(ElementName = "AmdmntInfDtls")]
    public class AmendmentInformationDetails
    {
        private OriginalCreditorSchemeIdentification _OriginalCreditorSchemeIdentification;
        /// <summary>
        /// Creditor Identifier auf dem Original SEPA Mandat
        /// </summary>
        [XmlElement(ElementName = "OrgnlCdtrSchmeId")]
        public OriginalCreditorSchemeIdentification OriginalCreditorSchemeIdentification
        {
            get
            {
                _OriginalCreditorSchemeIdentification = _OriginalCreditorSchemeIdentification ?? new OriginalCreditorSchemeIdentification();
                return _OriginalCreditorSchemeIdentification;
            }
            set
            {
                _OriginalCreditorSchemeIdentification = value;
            }
        }
    }

    /// <summary>
    /// Mandatsbezogene Informationen
    /// </summary>
    [XmlRoot(ElementName = "MndtRltdInf")]
    public class MandateRelatedInformation
    {
        /// <summary>
        /// Maximal 35 Zeichen
        /// Datenattribut gemäß EPC SEPA Regelwerk: AT-01 Eindeutige SEPA Mandatsreferenz
        /// </summary>
        [XmlElement(ElementName = "MndtId")]
        public string MandateIdentification { get; set; }

        /// <summary>
        /// Datum Unterzeichnung SEPA Mandat Datenattribut gemäß EPC SEPA Regelwerk: AT-25 Datum der Unterschrift auf SEPA Mandat ISO Date Time: Format YYYY-MM-TTT00:00:00
        /// </summary>
        [XmlElement(ElementName = "DtOfSgntr", DataType = "date")]
        public DateTime? DateOfSignature { get; set; }

        /// <summary>
        /// Kennzeichnung, ob das SEPA Mandat verändert wurde Bei Änderung SEPA Mandat ein Pflichtfeld = Konstante TRUE
        /// </summary>
        [XmlElement(ElementName = "AmdmntInd")]
        public bool AmendmentIndicator { get; set; } = false;

        [XmlIgnore]
        private AmendmentInformationDetails _AmendmentInformationDetails;
        /// <summary>
        /// Details SEPA Mandatsänderung Datenattribut gemäß EPC SEPA Regelwerk: AT-24 Grund für SEPA Mandatsänderung Pflichtfeld, wenn Amendment Indicator = TRUE
        /// </summary>
        [XmlElement(ElementName = "AmdmntInfDtls")]
        public AmendmentInformationDetails AmendmentInformationDetails
        {
            get
            {
                if (AmendmentIndicator)
                {
                    _AmendmentInformationDetails = _AmendmentInformationDetails ?? new AmendmentInformationDetails();
                }
                return _AmendmentInformationDetails;
            }
            set
            {
                _AmendmentInformationDetails = value;
            }
        }
    }

    /// <summary>
    /// Einzelne Lastschrifttransaktion
    /// </summary>
    [XmlRoot(ElementName = "DrctDbtTx")]
    public class DirectDebitTransaction
    {
        [XmlIgnore]
        private MandateRelatedInformation _MandateRelatedInformation;
        /// <summary>
        /// Mandatsbezogene Informationen
        /// </summary>
        [XmlElement(ElementName = "MndtRltdInf")]
        public MandateRelatedInformation MandateRelatedInformation
        {
            get
            {
                _MandateRelatedInformation = _MandateRelatedInformation ?? new MandateRelatedInformation();
                return _MandateRelatedInformation;
            }
            set
            {
                _MandateRelatedInformation = value;
            }
        }
    }

    /// <summary>
    /// Bank des Zahlungspflichtigen
    /// </summary>
    [XmlRoot(ElementName = "DbtrAgt")]
    public class DebtorAgent
    {
        [XmlIgnore]
        private FinancialInstitutionIdentification _FinancialInstitutionIdentification;
        /// <summary>
        /// Eindeutige ID einer Bank
        /// </summary>
        [XmlElement(ElementName = "FinInstnId")]
        public FinancialInstitutionIdentification FinancialInstitutionIdentification
        {
            get
            {
                _FinancialInstitutionIdentification = _FinancialInstitutionIdentification ?? new FinancialInstitutionIdentification();
                return _FinancialInstitutionIdentification;
            }
            set
            {
                _FinancialInstitutionIdentification = value;
            }
        }
    }

    /// <summary>
    /// Zahlungspflichtiger
    /// </summary>
    [XmlRoot(ElementName = "Dbtr")]
    public class Debtor
    {
        private string _name;
        [XmlElement(ElementName = "Nm")]
        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value.Replace("ä", "ae").Replace("ö", "oe").Replace("ü", "ue").Replace("Ä", "AE").Replace("Ö", "OE").Replace("Ü", "UE").Replace("ß", "ss");
            }
        }
    }

    /// <summary>
    /// Konto des Schuldners
    /// </summary>
    [XmlRoot(ElementName = "DbtrAcct")]
    public class DebtorAccount
    {

        [XmlIgnore]
        private Id _Id;
        [XmlElement(ElementName = "Id")]
        public Id Id
        {
            get
            {
                _Id = _Id ?? new Id();
                return _Id;
            }
            set
            {
                _Id = value;
            }
        }
    }

    /// <summary>
    /// Zahlungspflichtiger sofern abweichend vom Kontoinhaber, z.B.Kind des Kontoinhabers
    /// </summary>
    [XmlRoot(ElementName = "UltmtDbtr")]
    public class UltimateDebtor
    {

        [XmlElement(ElementName = "Nm")]
        public string Name { get; set; }

    }

    /// <summary>
    /// Verwendungszweck Datenattribut gemäß EPC SEPA Regelwerk: AT-22 Verwendungszweckangaben
    /// </summary>
    [XmlRoot(ElementName = "RmtInf")]
    public class RemittanceInformation
    {
        /// <summary>
        /// Unstrukturierter SEPA Verwendungszweck
        /// </summary>
        [XmlElement(ElementName = "Ustrd")]
        public string Unstructured { get; set; }
    }

    /// <summary>
    /// Einzeltransaktionen
    /// </summary>
    [XmlRoot(ElementName = "DrctDbtTxInf")]
    public class DirectDebitTransferTransactionInformation
    {
        [XmlIgnore]
        private PaymentIdentification _PaymentIdentification;

        /// <summary>
        /// Eindeutige Referenz des Einreichers an seine Bank
        /// </summary>
        [XmlElement(ElementName = "PmtId")]
        public PaymentIdentification PaymentIdentification
        {
            get
            {
                _PaymentIdentification = _PaymentIdentification ?? new PaymentIdentification();
                return _PaymentIdentification;
            }
            set { _PaymentIdentification = value; }
        }


        [XmlIgnore]
        private InstructedAmount _InstructedAmount;

        /// <summary>
        /// Zahlungsbetrag Datenattribut gemäß EPC SEPA Regelwerk: AT-06 Einzugsbetrag SEPA Lastschrift
        /// </summary>
        [XmlElement(ElementName = "InstdAmt")]
        public InstructedAmount InstructedAmount
        {
            get
            {
                _InstructedAmount = _InstructedAmount ?? new InstructedAmount();
                return _InstructedAmount;
            }
            set
            {
                _InstructedAmount = value;
            }
        }

        [XmlIgnore]
        private DirectDebitTransaction _DirectDebitTransaction;
        /// <summary>
        /// Einzelne Lastschrifttransaktion
        /// </summary>
        [XmlElement(ElementName = "DrctDbtTx")]
        public DirectDebitTransaction DirectDebitTransaction
        {
            get
            {
                _DirectDebitTransaction = _DirectDebitTransaction ?? new DirectDebitTransaction();
                return _DirectDebitTransaction;
            }
            set
            {
                _DirectDebitTransaction = value;
            }
        }

        private DebtorAgent _DebtorAgent;
        /// <summary>
        /// Bank des Zahlungspflichtigen
        /// </summary>
        [XmlElement(ElementName = "DbtrAgt")]
        public DebtorAgent DebtorAgent
        {
            get
            {
                _DebtorAgent = _DebtorAgent ?? new DebtorAgent();
                return _DebtorAgent;
            }
            set
            {
                _DebtorAgent = value;
            }
        }

        [XmlIgnore]
        private Debtor _Debtor;
        /// <summary>
        /// Zahlungspflichtiger (Pflichtfeld)
        /// </summary>
        [XmlElement(ElementName = "Dbtr")]
        public Debtor Debtor
        {
            get
            {
                _Debtor = _Debtor ?? new Debtor();
                return _Debtor;
            }
            set
            {
                _Debtor = value;
            }
        }

        [XmlIgnore]
        private DebtorAccount _DebtorAccount;
        /// <summary>
        /// Konto des Schuldners
        /// </summary>
        [XmlElement(ElementName = "DbtrAcct")]
        public DebtorAccount DebtorAccount
        {
            get
            {
                _DebtorAccount = _DebtorAccount ?? new DebtorAccount();
                return _DebtorAccount;
            }
            set
            {
                _DebtorAccount = value;
            }
        }

        [XmlIgnore]
        private UltimateDebtor _UltimateDebtor;
        /// <summary>
        /// Zahlungspflichtiger sofern abweichend vom Kontoinhaber, z.B.Kind des Kontoinhabers
        /// </summary>
        [XmlElement(ElementName = "UltmtDbtr")]
        public UltimateDebtor UltimateDebtor
        {
            get
            {
                _UltimateDebtor = _UltimateDebtor ?? new UltimateDebtor();
                return _UltimateDebtor;
            }
            set
            {
                _UltimateDebtor = value;
            }
        }

        [XmlIgnore]
        private RemittanceInformation _RemittanceInformation;
        /// <summary>
        /// Verwendungszweck Datenattribut gemäß EPC SEPA Regelwerk: AT-22 Verwendungszweckangaben
        /// </summary>
        [XmlElement(ElementName = "RmtInf")]
        public RemittanceInformation RemittanceInformation
        {
            get
            {
                _RemittanceInformation = _RemittanceInformation ?? new RemittanceInformation();
                return _RemittanceInformation;
            }
            set
            {
                _RemittanceInformation = value;
            }
        }
        public DirectDebitTransferTransactionInformation() { }

        public DirectDebitTransferTransactionInformation(string DebtorName, string DebtorIban, string DebtorBic, string MandateIdentification, double InstructedAmount, DateTime DateOfSignature, string RemittanceInformation)
        {
            this.InstructedAmount.instructedAmount = InstructedAmount;
            this.DirectDebitTransaction.MandateRelatedInformation.MandateIdentification = MandateIdentification;
            this.Debtor.Name = DebtorName;
            this.DebtorAccount.Id.IBAN = DebtorIban;
            this.DebtorAgent.FinancialInstitutionIdentification.BIC = DebtorBic;
            this.DirectDebitTransaction.MandateRelatedInformation.DateOfSignature = DateOfSignature.Date;
            this.RemittanceInformation.Unstructured = RemittanceInformation;
        }

    }

    /// <summary>
    /// Diverse Angaben, die für alle Einzeltransaktionen gelten: z.B.SEPA Auftraggeberkonto, SEPA Ausführungstermin
    /// </summary>
    [XmlRoot(ElementName = "PmtInf")]
    public class PaymentInformation
    {
        /// <summary>
        /// Maximal 35 Zeichen
        /// Referenz zur eindeutigen Identifizierung eines Sammlers im Kontoauszug (= DTA Feld A10)
        /// </summary>
        [XmlElement(ElementName = "PmtInfId")]
        public string PaymentInformationIdentification { get; set; } = DateTime.Now.ToFileTimeUtc().ToString();

        /// <summary>
        /// 
        /// </summary>
        [XmlElement(ElementName = "PmtMtd")]
        public string PaymentMethod { get; set; } = "DD";

        /// <summary>
        /// 
        /// </summary>
        [XmlElement(ElementName = "BtchBookg")]
        public bool BtchBookg { get; set; } = false;

        /// <summary>
        /// Maximal 15 nummerische Stellen
        /// 
        /// Anzahl der einzelnen Transaktionen innerhalb der ganzen SEPA Nachricht
        /// </summary>
        [XmlElement(ElementName = "NbOfTxs")]
        public int NumberOfTransactions { get; set; }

        /// <summary>
        /// Summe aller Einzeltransaktionen in der gesamten SEPA Nachricht
        /// </summary>
        [XmlElement(ElementName = "CtrlSum")]
        public double ControlSum { get; set; }

        [XmlIgnore]
        private PaymentTypeInformation _PaymentTypeInformation;
        /// <summary>
        /// SEPA Transaktionstyp
        /// </summary>
        [XmlElement(ElementName = "PmtTpInf")]
        public PaymentTypeInformation PaymentTypeInformation
        {
            get
            {
                _PaymentTypeInformation = _PaymentTypeInformation ?? new PaymentTypeInformation();
                return _PaymentTypeInformation;
            }
            set
            {
                _PaymentTypeInformation = value;
            }
        }

        /// <summary>
        /// Datenattribut gemäß EPC SEPA Regelwerk: AT-11 SEPA Fälligkeitsdatum der SEPA Lastschrift ISO Date Format YYYY-MM-DD
        /// </summary>
        [XmlElement(ElementName = "ReqdColltnDt", DataType = "date")]
        public DateTime? RequestedExecutionDate { get; set; } = DateTime.Now.AddDays(7).Date;

        [XmlIgnore]
        private Creditor _Creditor;
        /// <summary>
        /// SEPA Zahlungsempfänger (SEPA Gläubiger)
        /// </summary>
        [XmlElement(ElementName = "Cdtr")]
        public Creditor Creditor
        {
            get
            {
                _Creditor = _Creditor ?? new Creditor();
                return _Creditor;
            }
            set
            {
                _Creditor = value;
            }
        }

        [XmlIgnore]
        private CreditorAccount _CreditorAccount;
        /// <summary>
        /// Konto Zahlungsempfänger
        /// </summary>
        [XmlElement(ElementName = "CdtrAcct")]
        public CreditorAccount CreditorAccount
        {
            get
            {
                _CreditorAccount = _CreditorAccount ?? new CreditorAccount();
                return _CreditorAccount;
            }
            set
            {
                _CreditorAccount = value;
            }
        }

        [XmlIgnore]
        private CreditorAgent _CreditorAgent;
        /// <summary>
        /// Bank des Zahlungsempfängers
        /// </summary>
        [XmlElement(ElementName = "CdtrAgt")]
        public CreditorAgent CreditorAgent
        {
            get
            {
                _CreditorAgent = _CreditorAgent ?? new CreditorAgent();
                return _CreditorAgent;
            }
            set
            {
                _CreditorAgent = value;
            }
        }

        /// <summary>
        /// SEPA Entgeltverrechnung
        /// </summary>
        [XmlElement(ElementName = "ChrgBr")]
        public string ChargeBearer { get; set; } = "SLEV";

        [XmlIgnore]
        private CreditorSchemeIdentification _CreditorSchemeIdentification;
        /// <summary>
        /// Creditor Identifier auf dem Original SEPA Mandat
        /// </summary>
        [XmlElement(ElementName = "CdtrSchmeId")]
        public CreditorSchemeIdentification CreditorSchemeIdentification
        {
            get
            {
                _CreditorSchemeIdentification = _CreditorSchemeIdentification ?? new CreditorSchemeIdentification();
                return _CreditorSchemeIdentification;
            }
            set
            {
                _CreditorSchemeIdentification = value;
            }
        }

        /// <summary>
        /// Einzeltransaktionen
        /// </summary>
        [XmlElement(ElementName = "DrctDbtTxInf")]
        public List<DirectDebitTransferTransactionInformation> DirectDebitTransferTransactionInformation { get; set; } = new List<DirectDebitTransferTransactionInformation>();
    }

    [XmlRoot(ElementName = "CstmrDrctDbtInitn")]
    public class CstmrDrctDbtInitn
    {
        /// <summary>
        /// Kenndaten zur gemeinsamen Nutzung für alle Transaktionen innerhalb der SEPA Nachricht
        /// </summary>
        [XmlElement(ElementName = "GrpHdr")]
        public GroupHeader GroupHeader { get; set; } = new GroupHeader();

        /// <summary>
        /// Diverse Angaben, die für alle Einzeltransaktionen gelten: z.B.SEPA Auftraggeberkonto, SEPA Ausführungstermin
        /// </summary>
        [XmlElement(ElementName = "PmtInf")]
        public PaymentInformation PaymentInformation { get; set; } = new PaymentInformation();


    }

    [XmlRoot(ElementName = "Document", Namespace = "urn:iso:std:iso:20022:tech:xsd:pain.008.001.02")]
    public class Document
    {
        private CstmrDrctDbtInitn _cstmrDrctDbtInitn = new CstmrDrctDbtInitn();
        [XmlElement(ElementName = "CstmrDrctDbtInitn", Namespace = "urn:iso:std:iso:20022:tech:xsd:pain.008.001.02")]
        public CstmrDrctDbtInitn cstmrDrctDbtInitn
        {
            get
            {
                _cstmrDrctDbtInitn.GroupHeader.NumberOfTransactions = _cstmrDrctDbtInitn.PaymentInformation.DirectDebitTransferTransactionInformation.Count();
                _cstmrDrctDbtInitn.GroupHeader.ControlSum = Math.Round((double)_cstmrDrctDbtInitn.PaymentInformation.DirectDebitTransferTransactionInformation.Sum(s => s.InstructedAmount.instructedAmount), 2);


                _cstmrDrctDbtInitn.PaymentInformation.NumberOfTransactions = _cstmrDrctDbtInitn.PaymentInformation.DirectDebitTransferTransactionInformation.Count();
                _cstmrDrctDbtInitn.PaymentInformation.ControlSum = Math.Round((double)_cstmrDrctDbtInitn.PaymentInformation.DirectDebitTransferTransactionInformation.Sum(s => s.InstructedAmount.instructedAmount), 2);

                return _cstmrDrctDbtInitn;
            }
            set
            {

                _cstmrDrctDbtInitn = value;
            }
        }

        [XmlAttribute(AttributeName = "schemaLocation", Namespace = "http://www.w3.org/2001/XMLSchema-instance")]
        public string schemaLocation { get; set; } = "urn:iso:std:iso:20022:tech:xsd:pain.008.001.02 pain.008.001.02.xsd";

        public Document() { }

        public Document(string CreditorName, string CreditorIban, string CreditorBic, string CreditorId, PaymentTypeInformation.SequenceTypeEnum sequenceType)
        {

            _cstmrDrctDbtInitn.GroupHeader.InitiatingParty.Name = CreditorName;

            _cstmrDrctDbtInitn.PaymentInformation.PaymentTypeInformation.SequenceType = sequenceType;
            _cstmrDrctDbtInitn.PaymentInformation.Creditor.Name = CreditorName;
            _cstmrDrctDbtInitn.PaymentInformation.CreditorAccount.Id.IBAN = CreditorIban;
            _cstmrDrctDbtInitn.PaymentInformation.CreditorAgent.FinancialInstitutionIdentification.BIC = CreditorBic;
            _cstmrDrctDbtInitn.PaymentInformation.CreditorSchemeIdentification.Id.PrivateIdentification.OtherIdentification.Id = CreditorId;
        }

        public void AddTransaction(DirectDebitTransferTransactionInformation directDebitTransferTransactionInformation)
        {
            _cstmrDrctDbtInitn.PaymentInformation.DirectDebitTransferTransactionInformation.Add(directDebitTransferTransactionInformation);

        }
        public void CreateXmlFile(string File)
        {
            using (XmlWriter writer = XmlWriter.Create(File, new XmlWriterSettings() { Indent = true, Encoding = new UTF8Encoding(false) }))
            {

                XmlSerializerNamespaces xns = new XmlSerializerNamespaces();
                xns.Add("xsi", "http://www.w3.org/2001/XMLSchema-instance");


                XmlSerializer serializer = new XmlSerializer(typeof(SEPA.Pain_008.Document), "urn:iso:std:iso:20022:tech:xsd:pain.008.001.02");
                serializer.Serialize(writer, this, xns);
            }
        }
    }

}

