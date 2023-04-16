using PluginInterface;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PluginApp
{
    public partial class Form1 : Form
    {
        Dictionary<string, IPlugin> plugins = new Dictionary<string, IPlugin>();
        Dictionary<string, string> versions = new Dictionary<string, string>();

        public Form1()
        {
            InitializeComponent();
            FindPlugins();
            CreateContextMenu();
        }
        private void FindPlugins()
        {
            var neededPlugins = (ConfigurationManager.AppSettings["Plugins"]).Split(' ').ToList();
            if (neededPlugins.Count == 1 && neededPlugins[0] == "")
                neededPlugins.RemoveAll(x=>true);
            // папка с плагинами
            string folder = System.AppDomain.CurrentDomain.BaseDirectory;

            // dll-файлы в этой папке
            string[] files = Directory.GetFiles(folder, "*.dll");

            foreach (string file in files)
                try
                {
                    Assembly assembly = Assembly.LoadFile(file);

                    foreach (Type type in assembly.GetTypes())
                    {
                        Type iface = type.GetInterface("PluginInterface.IPlugin");

                        if (iface != null && (neededPlugins.Contains(type.Name) || neededPlugins.Count == 0))
                        {
                            IPlugin plugin = (IPlugin)Activator.CreateInstance(type);
                            plugins.Add(plugin.Name, plugin);
                            var attrs = System.Attribute.GetCustomAttributes(type);
                            foreach (System.Attribute attr in attrs)
                            {
                                if (attr is VersionAttribute)
                                {
                                    VersionAttribute a = (VersionAttribute)attr;
                                    versions.Add(plugin.Name, a.Major + " " + a.Minor);
                                }
                            }
                           
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка загрузки плагина\n" + ex.Message);
                }
        }
        private void OnPluginClick(object sender, EventArgs args)
        {
            IPlugin plugin = plugins[((ToolStripMenuItem)sender).Text];
            pictureBox1.Image = plugin.Transform((Bitmap)pictureBox1.Image);
        }
        private void CreateContextMenu()

        {
            var item = new ToolStripMenuItem("Плагины");

            foreach (var name in plugins.Keys)
            {
                var pluginItem = new ToolStripMenuItem(name, null, OnPluginClick);
                item.DropDownItems.Add(pluginItem);
                menuStrip1.Items.Add(item);
            }
        }
        private void button1_Click_1(object sender, EventArgs e)
        {
            using (OpenFileDialog dlg = new OpenFileDialog())
            {
                dlg.InitialDirectory = System.AppDomain.CurrentDomain.BaseDirectory;
                dlg.Title = "Open Image";
                dlg.Filter = "Image Files (*.bmp;*.jpg;*.jpeg,*.png)|*.BMP;*.JPG;*.JPEG;*.PNG";

                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    pictureBox1.Image = new Bitmap(dlg.FileName);
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var Text = "Список всех загруженных плагинов\n";
            foreach(var item in plugins)
            {
                
                Text += $"{item.Key}\t{versions[item.Key]}\t{item.Value.Author}\n";
            }
            MessageBox.Show(Text);
        }
    }
}
