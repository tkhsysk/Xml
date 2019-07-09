using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;

namespace Xml
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DataSet1 ds = new DataSet1();

            // tableもつくられる
            var count = ds.Tables.Count;

            ds.Tables[0].Rows.Add(DateTime.Now, 100);

            Debug.WriteLine(ds.Tables[0].Rows[0].Field<DateTime>("Date"));
            Debug.Indent();
            Debug.WriteLine(ds.Tables[0].Rows[0].Field<int>("Value"));
            Debug.Unindent();

            ds.WriteXml("ds.xml");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var ds = new DataSet1();
            ds.ReadXml("ds.xml");

            Debug.WriteLine(ds.Tables[0].Rows[0].Field<DateTime>("Date"));
            Debug.Indent();
            Debug.WriteLine(ds.Tables[0].Rows[0].Field<int>("Value"));
            Debug.Unindent();

            dataGridView1.DataSource = ds.Tables[0];
        }

        private void button3_Click(object sender, EventArgs e)
        {
            var items = new List<Item>()
            {
                new Item()
                {
                    Id = 1,
                    Name = "テレビ"
                },
                new Item()
                {
                    Id = 2,
                    Name = "ラジオ"
                }
            };

            var elements = items.Select(x =>
                new XElement("item",
                    new XElement("id", x.Id),
                    new XElement("name", x.Name)));

            var root = new XElement("items", elements);
            var xdoc = new XDocument(root);

            xdoc.Save("item.xml");
        }

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                var xdoc = XDocument.Load("item.xml");
                var xelements = xdoc.Root.Elements();

                foreach (var item in xelements)
                {
                    XElement xname = item.Element("name");
                    XElement xname2 = item.Element("id");

                    Debug.WriteLine(xname.Value);
                    Debug.WriteLine(xname2.Value);
                }
            }
            catch (XmlException ex)
            {
                Debug.WriteLine(ex.Message);
            }
            catch (NullReferenceException ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            // 名前空間がついた場合
            // http://neareal.net/index.php?Programming%2F.NetFramework%2FLINQ%2FLinqAndXmlNamespace

            var xdoc = XDocument.Load("ds.xml");

            // 名前空間
            var xns = xdoc.Root.Name.Namespace;

            var xelements = xdoc.Root.Elements();

            foreach(var x in xelements)
            {
                // DataSetの場合、名前空間の指定必要
                var date = x.Element(xns + "Date");
                var value = x.Element(xns + "Value");

                Debug.WriteLine(date.Value);
                Debug.WriteLine(value.Value);
            }
        }
    }
}
