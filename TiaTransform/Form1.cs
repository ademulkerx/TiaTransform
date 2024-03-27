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
                    var data = PlcTagImportFromText_V2(Txt_Plc.Text);
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
            int startIndex = data.IndexOf("STRUCT") + "STRUCT".Length;
            int endIndex = data.IndexOf("END_STRUCT;");
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

        public string PlcTagImportFromText_V2(string data)
        {
            int startIndex = data.IndexOf("STRUCT");
            int endIndex = data.IndexOf("END_STRUCT;");

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
                        case "string": // String zaten işlenmiş
                            break;
                        case "Time":
                            type = "System.TimeSpan";
                            break;
                        default:
                            type = "BilinmeyenTip"; // Bilinmeyen tip için placeholder kullan
                            break;
                    }

                    classBuilder.AppendLine($"    public {type} {name} {{ get; set; }}");
                }
            }

            classBuilder.AppendLine("}");

            return classBuilder.ToString();
        }



        #endregion



    }
}
