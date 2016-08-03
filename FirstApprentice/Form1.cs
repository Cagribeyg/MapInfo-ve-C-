using MapInfo;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FirstApprentice
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }


        public static MapInfoApplication mi;//dinamik olarak bir mi objesi tanımlıyoruz
      
        //mi objesini kullanarak artık tüm MapInfo uygulamalarını kullanabiliriz.
        private void Form1_Load(object sender, EventArgs e)
        {
            mi = new MapInfoApplication();//daha önce yaratılan mi objesine burada yön //verip onun bir MapInfoApplication ı olduğunu ifade eder
            int h = panel1.Handle.ToInt32();//
            //her panelin bir handle numarası var ve bu no Int32 formuna çevrildi
            mi.Do("Set Next Document Parent " + h.ToString() + " Style 1");
            mi.Do("set application window " + h.ToString());
            string ws_path = Application.StartupPath + "\\" + "LESSON1.wor";//MapInfo'dan projeye olustuşturduğumuz workspace
            mi.Do("Run Application \"" + ws_path + "\"");//uygulamanın akitive //olmasını sağlar }
            mi.Do("select il_adi from iller where il_adi<>\"\" order by il_adi into seliller noselect");
            int count = int.Parse(mi.Eval("tableinfo(seliller,8)"));
            mi.Do("fetch first from seliller");
            for (int i = 0; i < count; i++)
            {
                comboBox1.Items.Add(mi.Eval("seliller.col1"));
                mi.Do("fetch next from seliller");
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            mi.Do("run menu command 1705");
        }
        private void button2_Click(object sender, EventArgs e)
        {
            mi.Do("run menu command 1706");
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            string ilAdi = comboBox1.Text;
            if (ilAdi != "İl Seçiniz")
            {
                mi.Do("select * from iller where il_adi=\"" + ilAdi + "\"  into selsec");

            }
            if (int.Parse(mi.Eval("tableinfo(selsec,8)")) > 0)
            {
                mi.Do("add map window frontwindow() layer selsec");
                mi.Do("set map window frontwindow() Zoom Entire Layer selsec");
                mi.Do("remove map window frontwindow() layer selsec");
            }
            //Yukarıdaki kısım comboboxtan seçim yapılan ilin üzerine zoom yaptırır.

            //Aşağıdaki kısım ise seçilen il adına göre ilçeleri listeletir ve combobox2 ye yani ilçeler comboboxına aktarır. 

            mi.Do("select ilceadi from ilceler where iladi=\"" + comboBox1.Text + "\" order by ilceadi into selilceler noselect");
            int count = int.Parse(mi.Eval("tableinfo(selilceler,8)"));
            mi.Do("fetch first from selilceler");
            for (int i = 0; i < count; i++)
            {
                comboBox2.Items.Add(mi.Eval("selilceler.col1"));
                mi.Do("fetch next from selilceler");

            }
        }


        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBox2.Items.Clear();
            mi.Do("select ilceadi from ilceler where iladi=\"" + comboBox1.Text + "\" order by ilceadi into selilceler noselect");
            string ilAdi = comboBox1.Text;
            if (ilAdi != "İl Seçiniz")
            {
                mi.Do("select * from iller where il_adi=\"" + ilAdi + "\"                 into selsec");


            }
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}