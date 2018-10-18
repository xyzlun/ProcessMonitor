using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Management;
using System.Data;
using System.Drawing;
using System.Media;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.Threading;
using System.Drawing.Drawing2D;

namespace ProcessMonitor
{
    public partial class Form1 : Form
    {
        public bool running = true;
        public string appPath = "";
        public string TimeSpan;
        public string RestartTime;
        public List<string> on_process_list = new List<string>();//��Ҫ���ӵĽ����б�

        public Form1()
        {
            InitializeComponent();
            button3.Visible = false;
            IniCheckListBox();
            TimeSpan = System.Configuration.ConfigurationManager.AppSettings["TimeSpan"];
            RestartTime = System.Configuration.ConfigurationManager.AppSettings["RestartTime"];
        }
        private void IniCheckListBox()
        {
            foreach (Process ps in Process.GetProcesses())
            {
                checkedListBox1.Items.Add(ps.ProcessName);
            }
        }

        private void MonitorStart()
        {
            FileStream fs = new FileStream("log.txt", FileMode.Create);
            StreamWriter sw = new StreamWriter(fs);
            //�½�һ��Stopwatch��������ͳ�Ƴ�������ʱ��
            Stopwatch watch = Stopwatch.StartNew();
            //��ȡ�������е����н���ID�ͽ�����,�����������ʹ�õĹ�������˽�й�����
            while (running)
            {
                foreach (Process ps in Process.GetProcesses())
                {
                    if (on_process_list.Contains(ps.ProcessName))
                    {
                        PerformanceCounter pf1 = new PerformanceCounter("Process", "Working Set - Private", ps.ProcessName);
                        //PerformanceCounter pf2 = new PerformanceCounter("Process", "Working Set", ps.ProcessName);
                        //sw.Write(string.Format("{0}:{1}  {2:N}KB\r\n", ps.ProcessName, "������(������)", ps.WorkingSet64 / 1024));
                        //sw.Write(string.Format("{0}:{1}  {2:N}KB\r\n", ps.ProcessName, "������        ", pf2.NextValue() / 1024));
                        sw.Write(string.Format("{0}:{1} ; {2:N}KB ; {3}\r\n", ps.ProcessName, " ˽�й����� ", pf1.NextValue() / 1024, DateTime.Now));
                    }
                }
                Thread.Sleep(Convert.ToInt32(TimeSpan));
            }
            watch.Stop();
            sw.Write("������أ�����ʱ�䣺" + watch.Elapsed.ToString());
            //��ջ�����
            sw.Flush();
            //�ر���
            sw.Close();
            fs.Close();
            MessageBox.Show("���̼�ؽ�����");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            running = true;
            foreach (Object obj in checkedListBox1.CheckedItems)
            {
                on_process_list.Add(obj.ToString());
            }
            Thread threadA = new Thread(MonitorStart);
            threadA.Name = "MonitorStart";
            threadA.Start();
            label1.Text = "���ڼ�ؽ���...";

        }

        private void button2_Click(object sender, EventArgs e)
        {
            running = false;
            label1.Text = "ֹͣ���";
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
            System.Environment.Exit(0); 
        }

 

        private void button3_Click(object sender, EventArgs e)
        {
            int total_memory = GetPhisicalMemory();
            Form2 frm2 = new Form2(total_memory);
            frm2.Show();
        }

        private static int GetPhisicalMemory()
        {
            ManagementObjectSearcher searcher = new ManagementObjectSearcher(); //���ڲ�ѯһЩ��ϵͳ��Ϣ�Ĺ������ 
            searcher.Query = new SelectQuery("Win32_PhysicalMemory", "", new string[] { "Capacity" });//���ò�ѯ���� 
            ManagementObjectCollection collection = searcher.Get(); //��ȡ�ڴ����� 
            ManagementObjectCollection.ManagementObjectEnumerator em = collection.GetEnumerator();

            int capacity = 0;
            while (em.MoveNext())
            {
                ManagementBaseObject baseObj = em.Current;
                if (baseObj.Properties["Capacity"].Value != null)
                {
                    try
                    {
                        capacity += int.Parse(baseObj.Properties["Capacity"].Value.ToString());
                    }
                    catch
                    {
                        Console.WriteLine("�д�������", "������Ϣ");
                        return 0;
                    }
                }
            }
            return capacity;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Thread threadB = new Thread(ManageApp);
            threadB.Name = "ManageApp";
            threadB.Start();
            label2.Text = "�����ػ�����...";
        }

        private void ManageApp()
        {
            appPath = textBox1.Text;
            List<string> process_name_list_before = new List<string>();
            List<string> process_name_list_after = new List<string>();
            List<string> app_process_name = new List<string>() ;
            foreach (Process ps in Process.GetProcesses())
            {
                process_name_list_before.Add(ps.ProcessName);
            }
            Process.Start(appPath);
            foreach (Process ps in Process.GetProcesses())
            {
                if (!process_name_list_before.Contains(ps.ProcessName))
                {
                    app_process_name.Add(ps.ProcessName);
                }
            }

            while (true)
            {
                foreach (Process ps in Process.GetProcesses())
                {
                    process_name_list_after.Add(ps.ProcessName);
                }
                foreach (string str in app_process_name)
                {
                    if (!process_name_list_after.Contains(str))
                    {
                        Process.Start(appPath);                        
                    }
                }
                Thread.Sleep(Convert.ToInt32(RestartTime));
            }
        }
    }
}