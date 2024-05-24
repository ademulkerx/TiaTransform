using DevExpress.Utils.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;



namespace TiaTransform
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }



        #region  SelectMod

        int selectMod = -1;
        private void radioGroup1_SelectedIndexChanged(object sender, EventArgs e)
        {
            selectMod = radioGroup1.SelectedIndex;
        }

        private void radioGroup1_Paint(object sender, PaintEventArgs e)
        {
            selectMod = radioGroup1.SelectedIndex;
        }

        #endregion



        #region Buton Kontrol

        private void Btn_Copy_Click(object sender, EventArgs e)
        {
            try
            {
                Clipboard.SetText(Txt_Pc.Text);
            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.Message, "Uyarı");
            }
        }


        private void Btn_Paste_Click(object sender, EventArgs e)
        {
            try
            {
                Txt_Plc.Text = Clipboard.GetText();
            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.Message, "Uyarı");
            }
        }



        private void Btn_Convert_Click(object sender, EventArgs e)
        {
            switch (selectMod)
            {

                case 0:      // Direkt DB
                    var data = PlcTagImportFromText_V3(Txt_Plc.Text, (int)Nmc_DbNumber.Value);
                    Txt_Pc.Text = data;
                    break;

                case 1:      // HMI Tags
                    break;

                case 2:      // Merkezi İzleme
                    break;

                case 3:      // Giriş/Çıkış (I/O) Durumu
                    break;

                case 4:      // Sürücü Parametreleri
                    break;

                case 5:      // İşlem Verileri
                    break;

                default:
                    break;
            }
        }

        #endregion



        #region Data Convert



        public string PlcTagImportFromText_v1(string data)
        {
            int startIndex = data.IndexOf("//TiaTransform\r\n   STRUCT") + "//TiaTransform\r\n   STRUCT".Length;
            int endIndex = data.IndexOf("   END_STRUCT;\r\n\r\n\r\nBEGIN;");
            string extractedData = data.Substring(startIndex, endIndex - startIndex).Trim();

            string[] lines = extractedData.Split(new[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);

            List<string> dataList = new List<string>();


            foreach (string line in lines)
            {
                // Her bir satırı ':' karakterine göre böler ve gereksiz boşlukları temizler
                string[] parts = line.Split(':');
                if (parts.Length > 1)
                {
                    string name = parts[0].Trim();
                    string type = parts[1].Trim().Split(' ')[0]; // ';' karakterinden önceki kısmı alır
                    dataList.Add($"{name} : {type}");
                }
            }



            // C# class yapısına uygun hale getirmek için bir StringBuilder kullanılabilir.
            var classBuilder = new StringBuilder();
            classBuilder.AppendLine($"public class DB{Nmc_DbNumber.Value}");
            classBuilder.AppendLine("{");


            foreach (string line in dataList)
            {
                string[] parts = line.Split(':');
                if (parts.Length > 1)
                {
                    string name = parts[0].Trim();
                    name = name.Replace(":", "");
                    string type = parts[1].Trim().Split(' ')[0];
                    type = type.Replace(";", "");

                    // String için özel durumu ele alma
                    int stringSize = -1; // String boyutu için varsayılan değer
                    if (type.StartsWith("String"))
                    {
                        var match = Regex.Match(type, @"\[(\d+)\]");
                        if (match.Success)
                        {
                            stringSize = int.Parse(match.Groups[1].Value);
                            type = "String"; // C# tipini string olarak ayarla
                        }
                    }

                    switch (type)
                    {
                        case "Bool":
                            type = "bool";
                            break;

                        case "Byte":
                            type = "byte";
                            break;

                        case "Word":
                            type = "ushort";
                            break;

                        case "DWord":
                            type = "uint";
                            break;

                        case "Int":
                            type = "short";
                            break;

                        case "DInt":
                            type = "int";
                            break;

                        case "String":
                            if (stringSize == -1) // Eğer boyut belirtilmemişse varsayılan bir değer kullan
                            {
                                stringSize = 254; // Varsayılan string boyutu
                            }
                            classBuilder.AppendLine($"    [S7String(S7StringType.S7String, {stringSize})]");
                            type = "string";
                            break;

                        case "Time":
                            type = "System.TimeSpan";
                            break;

                        default:
                            type = "BilinmeyenTip"; // Bilinmeyen tip için genel bir tip kullan
                            break;
                    }

                    classBuilder.AppendLine($"    public {type} {name} {{ get; set; }}");

                }
            }


            classBuilder.AppendLine("}");



            return classBuilder.ToString();
        }


        // V2 çalışıyor tek eksik şimdilik struc yapı
        public string PlcTagImportFromText_V2(string data)
        {
            int startIndex = data.IndexOf("//TiaTransform\r\n   STRUCT") + "//TiaTransform\r\n   STRUCT".Length;
            int endIndex = data.IndexOf("BEGIN") - 2;

            if (startIndex == -1 || endIndex == -1 || endIndex < startIndex)
            {
                return "Veri içerisinde STRUCT veya END_STRUCT bulunamadı.";
            }

            string extractedData = data.Substring(startIndex + "STRUCT".Length, endIndex - startIndex - "STRUCT".Length).Trim();

            if (string.IsNullOrWhiteSpace(extractedData))
            {
                return "STRUCT ve END_STRUCT arasında veri bulunamadı.";
            }

            string[] lines = extractedData.Split(new[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);

            List<string> dataList = new List<string>();

            foreach (string line in lines)
            {
                string[] parts = line.Split(':');
                if (parts.Length > 1)
                {
                    string name = parts[0].Trim();
                    string type = parts[1].Trim().Split(' ')[0];

                    if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(type))
                    {
                        continue; // İsim veya tip boşsa bu satırı atla
                    }

                    dataList.Add($"{name} : {type}");
                }
                else
                {
                    continue; // ':' içermeyen satırları atla
                }
            }

            if (!dataList.Any())
            {
                return "Uygun veri satırları bulunamadı.";
            }

            var classBuilder = new StringBuilder();
            classBuilder.AppendLine($"public class DB{Nmc_DbNumber.Value}");
            classBuilder.AppendLine("{");

            foreach (string line in dataList)
            {
                string[] parts = line.Split(':');
                if (parts.Length > 1)
                {
                    string name = parts[0].Trim().Replace(":", "");
                    string type = parts[1].Trim().Split(' ')[0].Replace(";", "");

                    int stringSize = 254; // Varsayılan string boyutu
                    if (type.StartsWith("String"))
                    {
                        var match = Regex.Match(parts[1].Trim(), @"\[(\d+)\]");
                        if (match.Success)
                        {
                            stringSize = int.Parse(match.Groups[1].Value);
                        }
                        type = "string";
                        classBuilder.AppendLine($"    [S7String(S7StringType.S7String, {stringSize})]");
                    }

                    switch (type)
                    {
                        case "Bool":
                            type = "bool";
                            break;

                        case "Byte":
                            type = "byte";
                            break;

                        case "Word":
                            type = "ushort";
                            break;

                        case "DWord":
                            type = "uint";
                            break;

                        case "Int":
                            type = "short";
                            break;

                        case "DInt":
                            type = "int";
                            break;

                        case "Real":
                            type = "float";
                            break;

                        case "Char": // String zaten işlenmiş(yukarıda)
                            type = "byte_";
                            break;

                        case "string": // String zaten işlenmiş(yukarıda)
                            break;

                        case "Time":
                            type = "int";
                            break;

                        case "Time_Of_Day":
                            type = "uint";
                            break;

                        case "Date":
                            type = "ushort";
                            break;

                        default:
                            type = "BilinmeyenTip"; // Bilinmeyen tip için placeholder kullan
                            break;
                    }

                    if (type == "byte_")
                    {
                        classBuilder.AppendLine($"    public byte {name} {{ get; set; }} // Örnek Kullanım: var CharData = (char)db{Nmc_DbNumber.Value}.{name}");
                    }
                    else if (type == "float")
                    {//db1.Data_3.ToString("0.00000")
                        classBuilder.AppendLine($"    public {type} {name} {{ get; set; }} // Örnek Kullanım: float RealData = db{Nmc_DbNumber.Value}.{name}.ToString('0.00000')");
                    }
                    else
                    {
                        classBuilder.AppendLine($"    public {type} {name} {{ get; set; }}");
                    }

                }
            }

            classBuilder.AppendLine("}");

            return classBuilder.ToString();
        }

        bool strucEnable = false;
        int strucSayac = 0;


        public string PlcTagImportFromText_V3(string data, int MainClassNumber)
        {
            var lines = data.Split(new[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);

            int startIndex = Array.FindIndex(lines, line => line.Contains("STRUCT"));
            int endIndex = Array.FindIndex(lines, line => line.Contains("BEGIN")) - 1;

            if (startIndex == -1 || endIndex == -1 || endIndex < startIndex)
            {
                // Hata durumu veya uyarı mesajı
            }
            else
            {
                // `startIndex + 1` dahil ve `endIndex -1 ` dahil aralığı al ve lines e aktar...
                lines = lines.Skip(startIndex + 1).Take(endIndex - startIndex).ToArray();
            }

            // Artık temiz bir veri var...

            var classDefinitions = new Stack<StringBuilder>();
            StringBuilder currentClass = new StringBuilder();

            string className = null;


            currentClass.AppendLine($"public class DB{MainClassNumber}");
            currentClass.AppendLine("{");



            foreach (var line in lines)
            {
                if (line.Contains("Struct"))// Eğer Struct ifadesi var ise direkt bir class oluşturulacak yok ise normal değişkenler oluşturulacak
                {
                    var Line = line.Replace(" ", "").Replace(";", "").Trim();

                    var _data = Line.Split(':');

                    string name = "";
                    string type = "";
                    if (_data.Length == 2)
                    {
                        name = _data[0];
                        type = _data[1];
                    }
                    currentClass.AppendLine($"    public _{name} {name} {{ get; set; }}");
                    currentClass.AppendLine($"public class _{name}");
                    currentClass.AppendLine("{");

                }

                else if (line.Contains("END_STRUCT"))
                {
                    currentClass.AppendLine("}");
                }

                else
                {
                    var Line = line.Replace(" ", "").Replace(";", "").Trim();

                    var _data = Line.Split(':');



                    string name = "";
                    string type = "";
                    if (_data.Length == 2)
                    {
                        name = _data[0];
                        type = _data[1];
                    }

                    if (type.Contains("Bool"))
                    {
                        type = "bool";
                        currentClass.AppendLine($"    public {type} {name} {{ get; set; }}");
                    }
                    else if (type.Contains("Byte"))
                    {
                        type = "byte";
                        currentClass.AppendLine($"    public {type} {name} {{ get; set; }}");
                    }
                    else if (type.Contains("Word"))
                    {
                        type = "ushort";
                        currentClass.AppendLine($"    public {type} {name} {{ get; set; }}");
                    }
                    else if (type.Contains("DWord"))
                    {
                        type = "uint";
                        currentClass.AppendLine($"    public {type} {name} {{ get; set; }}");
                    }
                    else if (type.Contains("LWord"))
                    {
                        type = "ulong";
                        currentClass.AppendLine($"    public {type} {name} {{ get; set; }}");
                    }
                    else if (type.Contains("Int"))
                    {
                        type = "short";
                        currentClass.AppendLine($"    public {type} {name} {{ get; set; }}");
                    }
                    else if (type.Contains("DInt"))
                    {
                        type = "int";
                        currentClass.AppendLine($"    public {type} {name} {{ get; set; }}");
                    }
                    else if (type.Contains("LInt"))
                    {
                        type = "long";
                        currentClass.AppendLine($"    public {type} {name} {{ get; set; }}");
                    }
                    else if (type.Contains("Real"))
                    {
                        type = "float";
                        currentClass.AppendLine($"    public {type} {name} {{ get; set; }}");
                    }
                    else if (type.Contains("LReal"))
                    {
                        type = "double";
                        currentClass.AppendLine($"    public {type} {name} {{ get; set; }}");
                    }
                    else if (type.Contains("Char")) // String zaten işlenmiş(yukarıda)
                    {
                        
                        type = "char";
                        currentClass.AppendLine($"    public {type} {name} {{ get; set; }}");
                    }
                    else if (type.Contains("String"))
                    {
                        var match = Regex.Match(type, @"\[(\d+)\]");
                        int stringSize = 254;
                        if (match.Success)
                        {
                            stringSize = int.Parse(match.Groups[1].Value);
                        }
                        // type = "string";
                        currentClass.AppendLine($"    [S7String(S7StringType.S7String, {stringSize})]");
                        currentClass.AppendLine($"    public string {name} {{ get; set; }}");
                    }
                    else if (type.Contains("Time"))
                    {
                        type = "int";
                        currentClass.AppendLine($"    public {type} {name} {{ get; set; }}");
                    }
                    else if (type.Contains("Time_Of_Day"))
                    {
                        type = "uint";
                        currentClass.AppendLine($"    public {type} {name} {{ get; set; }}");
                    }
                    else if (type.Contains("Date"))
                    {
                        type = "ushort";
                        currentClass.AppendLine($"    public {type} {name} {{ get; set; }}");
                    }
                    else
                    {
                        type = "BilinmeyenTip"; // Bilinmeyen tip için placeholder kullan
                        currentClass.AppendLine($"    public {type} {name} {{ get; set; }}");
                    }




                    // güüncelleme yaparken eğer array tipinde veri okumaları yaparsak tanımlamarak bu şekilde olacak:
                    // Burada 10 elemanlı bir array double tanımladık.
                    //public double[] ValuesLREAL { get; set; } = new double[10];

                }
            }

            return currentClass.ToString();

        }

        private string ConvertPlcTypeToCSharpType(string plcType)
        {
            switch (plcType)
            {
                case "Bool":
                    return "bool";

                case "Byte":
                    return "byte";

                case "Word":
                    return "ushort";

                case "DWord":
                    return "uint";

                case "Int":
                    return "short";

                case "DInt":
                    return "int";

                case "Real":
                    return "float";

                case "S5Time":
                case "Time":
                case "Time_Of_Day":
                case "Date_And_Time":
                    return "DateTime";

                case "Char":
                case "S7Char":
                    return "char";

                case "String":
                    return "string";

                // Burada daha fazla PLC tipi ekleyebilirsiniz
                default:
                    return "object"; // Bilinmeyen veya desteklenmeyen tipler için genel bir tip
            }
        }




        #endregion

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
