using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;
using PKHeX.Core;
using RaidCrawler.Structures;
using SysBot.Base;

namespace RaidCrawler.Subforms
{
    public partial class EmojiConfig : Form
    {
        private readonly Config c = new();
        public EmojiConfig(Config c)
        {
            InitializeComponent();

            this.c = c;

            dataGridView1.DataSource = EmojiLoad(c.Emoji);
        }

        private Image EmoteParse(string text)
        {
            if (TryParse(text, out ArrayList result))
            {
                string urlBase = "https://cdn.discordapp.com/emojis/";
                string url = $"{urlBase}{result[0]}"+((bool)result[1] ? ".gif" : ".png" )+ "?v=1";

                HttpWebRequest discoRequest = (HttpWebRequest)WebRequest.Create(url);
                discoRequest.Method = "GET";
                HttpWebResponse discoResponse = (HttpWebResponse)discoRequest.GetResponse();
                System.Drawing.Bitmap bmp = new System.Drawing.Bitmap(discoResponse.GetResponseStream());
                Bitmap bmp2 = new System.Drawing.Bitmap(bmp,new Size(20, 20));
                
                discoResponse.Close();

                return bmp2;
            }
            string urlE = "https://cdn.discordapp.com/emojis/1063366847078076506.png?v=1";
            HttpWebRequest emptyRequest = (HttpWebRequest)WebRequest.Create(urlE);
            emptyRequest.Method = "GET";
            HttpWebResponse emptyResponse = (HttpWebResponse)emptyRequest.GetResponse();
            System.Drawing.Bitmap empt = new System.Drawing.Bitmap(emptyResponse.GetResponseStream());
            emptyResponse.Close();
            return empt;
        }

        private static bool TryParse(string text, out ArrayList result)
        {
            result = null;

            if (text == null)
                return false;

            if (text.Length >= 4 && text[0] == '<' && (text[1] == ':' || (text[1] == 'a' && text[2] == ':')) && text[text.Length - 1] == '>')
            {
                bool animated = text[1] == 'a';
                int startIndex = animated ? 3 : 2;

                int splitIndex = text.IndexOf(':', startIndex);
                if (splitIndex == -1)
                    return false;

                if (!ulong.TryParse(text.Substring(splitIndex + 1, text.Length - splitIndex - 2), NumberStyles.None, CultureInfo.InvariantCulture, out ulong id))
                    return false;

                string name = text.Substring(startIndex, splitIndex - startIndex);
                result = new ArrayList();
                result.Add(id);
                result.Add(animated);
                    
                result.Add(name);
                return true;
            }
            return false;
        }

        private DataTable EmojiLoad(Dictionary<string, string> emoji)
        {
            DataTable d = new DataTable();
            d.Columns.Add("Emoji", typeof(string));
            d.Columns.Add("Emoji Value", typeof(string));
            emoji.ToList().ForEach(KeyValuePair => d.Rows.Add(new object[] { KeyValuePair.Key, KeyValuePair.Value }));
            d.Columns[0].ReadOnly = true;
            d.Columns.Add("📷", typeof(Image));
            int i = 0;
            emoji.ToList().ForEach(KeyValuePair => d.Rows[i++]["📷"] = EmoteParse(KeyValuePair.Value));

            d.Columns[2].ReadOnly = true;
            return d;
        }

        private Dictionary<string, string> EmojiSave(DataTable emoji)
        {
            Dictionary<string, string> d = new Dictionary<string, string>();
            emoji.AsEnumerable().ToList().ForEach(row => d.Add(row[0] as string, row[1] as string));
            return d;
        }

        private void Save_Click(object sender, EventArgs e)
        {
            c.Emoji = EmojiSave((DataTable)dataGridView1.DataSource);
           
            string output = JsonConvert.SerializeObject(c);
            using StreamWriter sw = new(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "config.json"));
            sw.Write(output);


        }
    }
}
