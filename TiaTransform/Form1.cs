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
                    //var data = PlcTagImportFromText_V3(Txt_Plc.Text, (int)Nmc_DbNumber.Value);
                    var data = GenerateClassFromPlcTags(Txt_Plc.Text, (int)Nmc_DbNumber.Value);
                    Txt_Pc.Text = data;
                    break;

                case 1:      // Tag - Offset

                    var tag = GenerateTagMapFromText(Txt_Plc.Text, (int)Nmc_DbNumber.Value);
                    Txt_Pc.Text = tag;


                    break;


                case 3:      // Giriş/Çıkış (I/O) Durumu
                    break;

                case 4:      // Sürücü Parametreleri
                    break;


                default:
                    break;
            }
        }

        #endregion

        public string convertType(string dataType)
        {
            string csharpType = "";
            if (dataType == "bool")
            {
                csharpType = "bool";
            }
            else if (dataType == "byte" || dataType == "usint" || dataType == "sint")
            {
                csharpType = "byte";
            }
            else if (dataType == "word")
            {
                csharpType = "ushort";
            }
            else if (dataType == "ınt")
            {
                csharpType = "short";
            }
            else if (dataType == "uint")
            {
                csharpType = "ushort";
            }
            else if (dataType == "dword")
            {
                csharpType = "uınt";
            }
            else if (dataType == "dınt")
            {
                csharpType = "int";
            }
            else if (dataType == "real")
            {
                csharpType = "float";
            }
            else if (dataType.StartsWith("string"))
            {
                csharpType = "string";
            }
            else
            {
                csharpType = ""; // Bilinmeyen tip
            }
            return csharpType;
        }

        #region Data Convert
        public string GenerateClassFromPlcTags(string rawText, int dbNumber)
        {
            var sb = new StringBuilder();
            sb.AppendLine($"public class DB{dbNumber}");
            sb.AppendLine("{");

            var lines = rawText.Split(new[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries);

            foreach (var line in lines)
            {
                var parts = line.Split('\t');
                if (parts.Length < 3)
                    continue;

                string name = parts[1].Trim();
                string dataType = parts[2].Trim().ToLower();

                string comment = "";
                if (parts.Length == 12)
                    comment = parts[11].Trim().ToLower();

                string csharpType;

                if (dataType == "bool")
                {
                    csharpType = "bool";
                }
                else if (dataType == "byte" || dataType == "usint" || dataType == "sint")
                {
                    csharpType = "byte";
                }
                else if (dataType == "word")
                {
                    csharpType = "ushort";
                }
                else if (dataType == "ınt")
                {
                    csharpType = "short";
                }
                else if (dataType == "uint")
                {
                    csharpType = "ushort";
                }
                else if (dataType == "dword")
                {
                    csharpType = "uınt";
                }
                else if (dataType == "dınt")
                {
                    csharpType = "int";
                }
                else if (dataType == "real")
                {
                    csharpType = "float";
                }
                else if (dataType.StartsWith("string"))
                {
                    var match = Regex.Match(dataType, @"\[(\d+)\]");
                    int stringSize = match.Success ? int.Parse(match.Groups[1].Value) : 254;


                    sb.AppendLine($"");
                    sb.AppendLine($"    [S7String(S7StringType.S7String, {stringSize})]");
                    csharpType = "string";
                }
                else
                {
                    continue; // Bilinmeyen tip
                }
                if (comment != "")
                    sb.AppendLine($"    /// <summary> {comment} </summary>");

                sb.AppendLine($"    public {csharpType} {name} {{ get; set; }}");
            }

            sb.AppendLine("}");
            return sb.ToString();
        }


        public string GenerateTagMapFromText(string rawText, int dbNumber)
        {
            var entries = new List<string>();
            var lines = rawText.Split(new[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries);

            foreach (var line in lines)
            {
                var parts = line.Split('\t');

                if (parts.Length < 4)
                    continue;

                string name = parts[1].Trim();
                string dataType = parts[2].Trim().ToLower();
                string offsetStr = parts[3].Trim();
                int strByteLength = 254; //Max string alanı
                bool strData = false;

                //Eğer type string ise 
                if (dataType.Contains("string"))
                {
                    strData = true;
                    if (dataType.Contains("["))//Alan belirtilmiş
                    {
                        strByteLength = int.Parse(dataType.Split('[')[1].Replace("[","").Replace("]", ""));
                    }
                    else
                    {
                        strByteLength = 254; //Max string alanı
                    }
                    dataType = "string";

                }

                if (!offsetStr.Contains("."))
                    continue;

                var offsetParts = offsetStr.Split('.');
                if (!int.TryParse(offsetParts[0], out int byteOffset))
                    continue;

                int bitOffset = (offsetParts.Length > 1 && int.TryParse(offsetParts[1], out int b)) ? b : 0;

                string Address = null;
                string DataType_plc = null;
                string DataType_pc = null;

                if (dataType == "bool")
                    Address = $"DB{dbNumber}.DBX{byteOffset}.{bitOffset}";

                else if (dataType == "byte" || dataType == "usınt" || dataType == "sınt")
                    Address = $"DB{dbNumber}.DBB{byteOffset}";

                else if (dataType == "word" || dataType == "ınt" || dataType == "uınt")
                    Address = $"DB{dbNumber}.DBW{byteOffset}";

                else if (dataType == "dword" || dataType == "dınt" || dataType == "real" || dataType == "float")
                    Address = $"DB{dbNumber}.DBD{byteOffset}";

                else if (dataType == "string")
                    Address = $"DB{dbNumber}.DBS{byteOffset}[{strByteLength}]";

                else
                    continue;

                DataType_plc = dataType;
                DataType_pc = convertType(dataType);


                if (string.IsNullOrEmpty(Address))
                    continue;

                //entries.Add($"    {{ \"{name}\",  new TagInfo(\"{Address}\", \"{DataType_plc}\", \"{DataType_pc}\") }}");
                entries.Add($"{{ \"{name}\", new TagInfo(\"{Address}\", \"{DataType_plc}\", \"{DataType_pc}\") }}");
            }

            //var sb = new StringBuilder();
            //sb.AppendLine($"        public static readonly Dictionary<string, TagInfo> db{dbNumber} = new Dictionary<string, TagInfo>");
            //sb.AppendLine("        {");
            //sb.AppendLine("            " + string.Join(",\n\t\t", entries));
            //sb.AppendLine("        };");
            var sb = new StringBuilder();
            sb.AppendLine($"\t\tpublic static readonly Dictionary<string, TagInfo> db{dbNumber} = new Dictionary<string, TagInfo>");
            sb.AppendLine("\t\t{"); // Bu satırda \t\t var

            // Ayırıcı: Virgülden sonra yeni satır, ardından üç tab boşluğu (\t\t\t)
            // Üç tab (örneğin) kullanarak tüm satırların aynı hizada olmasını sağlayın.
            string separator = ",\n\t\t\t";

            // 1. İlk elemanın başına gelecek girintiyi (varsayımsal olarak \t\t\t) ekleyin
            sb.Append("\t\t\t");

            // 2. Birleştirilmiş listeyi ekleyin, ayırıcı diğer elemanların girintisini sağlar
            sb.Append(string.Join(separator, entries));

            sb.AppendLine(); // Son elemandan sonraki satır atlaması
            sb.AppendLine("\t\t};");

            return sb.ToString();
        }


        #endregion

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
