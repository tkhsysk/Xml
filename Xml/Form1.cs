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
            // 名前空間がついた場合
            // http://neareal.net/index.php?Programming%2F.NetFramework%2FLINQ%2FLinqAndXmlNamespace

            try
            {
                var xdoc = XDocument.Load("item.xml");
                var xelements = xdoc.Root.Elements();

                foreach (var item in xelements)
                {
                    XElement xname = item.Element("name");
                    XElement xname2 = item.Element("id");

                    System.Diagnostics.Debug.WriteLine(xname.Value);
                    System.Diagnostics.Debug.WriteLine(xname2.Value);
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
    }
}
