using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.Text.RegularExpressions;
using System.Threading;
using System.Net;
using System.IO;
using System.Web;

namespace 句词下载
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void saveFileDialog1_FileOk(object sender, CancelEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            //创建一个线程
            Thread thread = new Thread(getresult);
            thread.Start();
        }

        private void getresult()
        {
            int i;
            FileStream fs = new FileStream(textBox5.Text.Equals("") ? "result.txt" : textBox5.Text, FileMode.Append);
            StreamWriter sw = new StreamWriter(fs, Encoding.GetEncoding("UTF-8"));
            label6.Text = "状态：进行中";
            for (i = Int32.Parse(textBox3.Text); i <= Int32.Parse(textBox4.Text); i++)
            {
                //发送请求
                HttpWebRequest myReq = (HttpWebRequest)WebRequest.Create(textBox1.Text.Replace("(*)",""+i));

                //获得响应
                string res = string.Empty;
                try
                {
                    //获取响应流
                    HttpWebResponse response = (HttpWebResponse)myReq.GetResponse();
                    StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding("UTF-8"));
                    res = reader.ReadToEnd();
                    reader.Close();
                    response.Close();
                    //保存源码
                    //FileStream fs = new FileStream("test.html", FileMode.Create);
                    //StreamWriter sw = new StreamWriter(fs, Encoding.GetEncoding("UTF-8"));
                    //sw.Write(res);
                    //sw.Close();
                    //fs.Close();
                    //正则表达式
                    string pattern = textBox2.Text.Replace("(*)","(?<name>.+?)");
                    MatchCollection matchs;
                    matchs = Regex.Matches(res, pattern,RegexOptions.Singleline);
                    sw.Write(matchs[0].Groups["name"]+"\r\n");
                    label6.Text = "状态："+i;
                }
                catch (Exception e)
                {
                }
            }
            label6.Text = "状态：完成";
            sw.Close();
            fs.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            saveFileDialog1.Filter = "Txt文本文件|*.txt";
            saveFileDialog1.Title = "保存";
            saveFileDialog1.ShowDialog();
            if (saveFileDialog1.FileName != "")
            {
                textBox5.Text = saveFileDialog1.FileName;
            }
        }
    }
}
