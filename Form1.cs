#include <stdio.h>
#include <assert.h>
#include <stdlib.h>
#include <time.h>
#include "ross.h"
#include "model.h"
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Collections;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Forms;
using System.Xml;
using Astar;
using AGV_V1._0.Properties;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using System.Xml.Linq;
using System.IO;
//using GMap.NET.WindowsForms;
//using GMap.NET;
//using GMap.NET.MapProviders;

namespace AGV_V1._0
{
    public partial class Form1 : Form
    {

        public Form1()
        {
            InitializeComponent();
            InitialGame();
        }

////////////////////////////////////////////Определения переменных///////////////////////////////////////////

        public Image img_Belt;                    //Место сбора
        public Image img_Road;                    //путь
        public Image img_Destination;             //место доставки
        public Image img_ChargeStation;           //зона зарядки
        public Image img_Obstacle;                //препятствие
        public Image img_Icon;                    //икона
        public Image img_Alter;                   //заменять
        public Image img_White;                   //Белое изображение, когда изображение графа узлов пустое, оно будет отображаться белым цветом того же цвета, что и фон .
        public Image img_Display;                //Статус при выборе в качестве пункта назначения
        public Image img_Orange;                //Статус при выборе в качестве пункта назначения
        public Image img_Yellow;

        //Создайте два новых глобальных объекта: автомобиль и электронную карту.
        Vehicle[] vehicle;

        static ElecMap Elc;

        //Название картинки для замены
        String PicString;

        //Нажмите на изображение, чтобы разместить изображение и введите
        Image SetImage;
        bool SetType;

        //Может ли машина запустить бит флага, правда может начаться
        bool Vehicle_Start;

        //Можно ли изменить флаг объекта, true можно изменить
        bool Object_Change;

        //Бит флага кнопки назначения, true 
        bool Destination_Get;

        //Флажковое положение кнопки тележки АГВ
        bool Vehicle_Get;

        //Кнопка добавления тележки AGV
        bool AGV_Add;

        //Элемент управления предотвращает нажатие кнопки запуска автомобиля и назначение работы, когда система остановлена.
        bool CanStart;

        //Совпадают ли автомобиль и пункт назначения, чтобы предотвратить только пункт назначения, но не автомобиль, или только автомобиль, но не пункт назначения.
        bool Des;
        bool Veh;

        //Используется для записи координат пункта назначения, на который нажали, и индекса автомобиля, на который нажали.
        static int Set_Destination_X;
        static int Set_Destination_Y;
        static int Vehicle_Index;

        Stack<float> Picture_Length;   //Стек -> хранить большие и маленькие картинки
        Stack<int> Panel_Width;       
        Stack<int> Panel_Height;      

        
        float small_number;

        
        int iWidth, iHeight;

        private Bitmap surface;
        private Graphics g;

        
        Form F_HegWethBech;

        
        Form F_VehicleRoute;
        Label L_VehicleRoute;

//////////////////////////////Установите переменную под кнопкой///////////////////////////////////

        Panel P_TopPanel;     
        Panel P_HWForm; 
        Panel P_HWMap;  
        Panel P_BottomPanel;

        Button B_HWForm;    //Кнопки высоты и ширины рамки
        Button B_HWMap;     //Кнопки длины и ширины карты
        Button B_OKBotton;  //Кнопка подтверждения внизу, подтвердите после установки длины и ширины рамки и длины и ширины карты
        Button B_CANCELBotton; //Кнопка отмены внизу, отмена после установки длины и ширины рамки и длины и ширины карты

        //Инициализировать поля ввода длины, ширины и ссылки в форме.
        TextBox T_HegForm;
        TextBox T_WethForm;
        TextBox T_Bech;
        TextBox T_HegMap;
        TextBox T_WethMap;

        //Инициализировать метку в левой части поля ввода
        Label L_HegForm;
        Label L_WethForm;
        Label L_Bech;
        Label L_HegMap;
        Label L_WethMap;


///////////////////////////////////////////////Переменная определена//////////////////////////////////////////////


        /// <summary>
        /// </summary>
        private void InitialGame()
        {
            InitVariable();    //Инициализировать переменные
            InitialElc();      //Инициализировать электронную карту
            InitialVehicle();   //Инициализировать автомобиль
            InitStack();       //Инициализировать стек
        }

        /// <summary>
       
        /// </summary>
        public void InitVariable()
        {
            img_Belt = Resources.Belt;                    
            img_Mid = Resources.Mid;                      
            img_Road = Resources.Road;                    
            img_Destination = Resources.Destination;      
            img_ChargeStation = Resources.ChargeStation;  
            img_Obstacle = Resources.Obstacle;            
            img_Scanner = Resources.Scanner;              
            img_Alter = Resources.Alter;                  
            img_Png = Resources.Png;                      
            img_White = Resources.White;                 
            img_Flag = Resources.Flag;                    
            img_Display = Resources.Display;
            img_Orange = Resources.Vehicle_Orange;
            img_Yellow = Resources.Vehicle_Yellow;


            Vehicle_Start = false;

            Object_Change = false;

            Destination_Get = false;

            Vehicle_Get = false;

            AGV_Add = false;

            CanStart = false;

            Des = false;
            Veh = false;

            surface = null;
            g = null;

            F_HegWethBech = new Form();

            T_HegForm = new TextBox();
            T_WethForm = new TextBox();
            T_Bech = new TextBox();

            L_HegForm = new Label();
            L_WethForm = new Label();
            L_Bech = new Label();

            F_VehicleRoute = new Form();
            L_VehicleRoute = new Label();

            P_TopPanel = new Panel();
            P_HWForm = new Panel();
            P_HWMap = new Panel();
            P_BottomPanel = new Panel();

            B_HWForm = new Button();
            B_HWMap = new Button();
            B_OKBotton = new Button();
            B_CANCELBotton = new Button();

            T_HegMap = new TextBox();
            T_WethMap = new TextBox();

            L_HegMap = new Label();
            L_WethMap = new Label();

            B_OKBotton.Click += new System.EventHandler(this.B_OKBotton_Click);
        }

        /// <summary>
        
        /// </summary>
        public void InitialXml()
        {
            
            string path = "../../XMLFile1.xml";

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(path);

            
            XmlNode root = xmlDoc.SelectSingleNode("config");

            
            XmlNode xn = root.SelectSingleNode("WIDTH");
            constDefine.WIDTH = Convert.ToInt32(xn.InnerText);

           
            xn = root.SelectSingleNode("HEIGHT");
            constDefine.HEIGHT = Convert.ToInt32(xn.InnerText);

           
            xn = root.SelectSingleNode("VEHICL_COUNT");
            constDefine.VEHICL_COUNT = Convert.ToInt32(xn.InnerText);

            
            xn = root.SelectSingleNode("Form_Height");
            constDefine.Form_Height = Convert.ToInt32(xn.InnerText);

           
            xn = root.SelectSingleNode("Form_Width");
            constDefine.Form_Width = Convert.ToInt32(xn.InnerText);

        }

        public void InitialElc()
        {

            Elc = new ElecMap(constDefine.WIDTH, constDefine.HEIGHT);

            Elc.heightNum = constDefine.HEIGHT / constDefine.BENCHMARK;
            Elc.widthNum = (constDefine.WIDTH - constDefine.BLANK_X) / constDefine.BENCHMARK;

            Elc.mapnode = new MapNode[Elc.heightNum, Elc.widthNum];
            Elc.TempMapNode = new MapNode[Elc.heightNum, Elc.widthNum];

            //Установите область прокрутки полосы прокрутки
            this.AutoScrollMinSize = new Size(constDefine.WIDTH + constDefine.BEGIN_X, constDefine.HEIGHT);

            //Инициализировать положение карты            
            Elc.SetObject();    //Инициализировать электронную карту и одновременно расположить объекты на электронной карте

            //Установите размер и положение pictureBox
            pic.Location = Point.Empty;
            pic.ClientSize = new System.Drawing.Size(constDefine.WIDTH, constDefine.HEIGHT);
            surface = new Bitmap(constDefine.WIDTH, constDefine.HEIGHT);
            g = Graphics.FromImage(surface);

            //Назначьте значение изображению и введите текст, размещенный на изображении, по которому щелкнули, каждому изображению, по которому щелкнули, по умолчанию назначается изображение дороги.
            SetImage = img_Road;
            SetType = true;

            
            panel1.ClientSize = new System.Drawing.Size(constDefine.WIDTH, constDefine.HEIGHT);
            panel2.ClientSize = new System.Drawing.Size(constDefine.PANEL_X, constDefine.HEIGHTPANEL2);

            Add_Panel2();

            
            pic.Image = surface;
            panel1.Controls.Add(pic);

            this.ClientSize = new System.Drawing.Size(constDefine.Form_Width, constDefine.Form_Height);
        }

        /// <summary>
        ///Инициализировать автомобиль
        /// </summary>
        public void InitialVehicle()
        {
           //Инициализировать положение автомобиля
            vehicle = new Vehicle[1000];

            //Начальные координаты автомобиля
            int startX;
            int startY;

            
            string Str_H;
            string Str_Z;

            for (int i = 0; i < constDefine.VEHICL_COUNT; i++)
            {
               
                
                string path = "../../XMLFile1.xml";

                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(path);

               
                XmlNode root = xmlDoc.SelectSingleNode("config");

                Str_Z = "agv" + (i+1).ToString() + "-Z";

                Str_H = "agv" + (i+1).ToString() + "-H";

                
                XmlNode xn = root.SelectSingleNode(Str_H);
                startY = Convert.ToInt32(xn.InnerText);

                xn = root.SelectSingleNode(Str_Z);
                startX = Convert.ToInt32(xn.InnerText);
                
                vehicle[i] = new Vehicle();

                vehicle[i] = new Vehicle(startX, startY, vehicle[i].img_Vehicle[5], false);

                vehicle[i].endX = 0;
                vehicle[i].endY = 0;

                //Elc.mapnode[startX, startY].nodeCanUsed = false;
                //Elc.TempMapNode[startX, startY].nodeCanUsed = false;

                vehicle[i].endX = vehicle[i].startX;
                vehicle[i].endY = vehicle[i].startY;
            }
            //Установите узел, в котором находится автомобиль, в состояние «занято».
            VehicleOcuppyNode();

            //путь поиска
            for (int i = 0; i < constDefine.VEHICL_COUNT; i++)
            {
                vehicle[i].SearchRoute(Elc, vehicle[i].startX, vehicle[i].startY, vehicle[i].endX, vehicle[i].endY);
            }

            //Обнаружение конфликтующих узлов и перенаправление
            CheckeConflictNode();
        }

        /// <summary>
        /// Инициализировать стек для хранения значений увеличения и уменьшения масштаба
        /// </summary>
        public void InitStack()
        {
            small_number = 1;                       
            Picture_Length = new Stack<float>();    //Инициализация стека каждого изображения узла
            Panel_Width = new Stack<int>();         
            Panel_Height = new Stack<int>();        
            Picture_Length.Push(small_number);
            iWidth = constDefine.WIDTH;             
            iHeight = constDefine.HEIGHT;           
            Panel_Width.Push(iWidth);
            Panel_Height.Push(iHeight);
        }

        /// <summary>
       
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            Draw(g);
            timer1.Start();
        }

        /// <summary>
        /// таймер
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timer1_Tick(object sender, EventArgs e)
        {
            if (Vehicle_Start)
            {
                for (int i = 0; i < constDefine.VEHICL_COUNT; i++)
                {
                    vehicle[i].Vehicle_Move(Elc);

                    //if ((vehicle[i].Y / constDefine.BENCHMARK) == vehicle[i].endX && (vehicle[i].X / constDefine.BENCHMARK) == vehicle[i].endY)
                    //{
                    //    Elc.TempMapNode[vehicle[i].endX, vehicle[i].endY].oth = Elc.mapnode[vehicle[i].endX, vehicle[i].endY].oth;
                    //}
                   
                }
            }
            
            this.Refresh();
        }

        /// <summary>
        ///Нарисовать электронную карту
        /// </summary>
        /// <param name="e"></param>
        public void Draw(Graphics g)
        {
            // Graphics g = e.Graphics;
            //нарисовать карту            
            for (int i = 0; i < Elc.heightNum; i++)
            {
                for (int j = 0; j < Elc.widthNum; j++)
                {
                    if (Elc.TempMapNode[i, j].oth == null)
                    {
                        Elc.TempMapNode[i, j].oth = img_White;
                    }
                        g.DrawImage(Elc.TempMapNode[i, j].oth, Elc.TempMapNode[i, j].x, Elc.TempMapNode[i, j].y);
                }
            }
           
            //нарисовать машину
            int count = Elc.heightNum;
            if (Elc.heightNum > constDefine.VEHICL_COUNT)
            {
                count = constDefine.VEHICL_COUNT;
            }
           
            for (int i = 0; i < count; i++)
            {
                vehicle[i].Draw(g,Elc);
                if(vehicle[i].route.Count-1==vehicle[i].routeIndex)
                {
                    Elc.TempMapNode[vehicle[i].endX, vehicle[i].endY].oth = Elc.mapnode[vehicle[i].endX, vehicle[i].endY].oth;
                }
            }
        }

        /// <summary>
        /// Легенда кнопки «Присоединиться» и т. д.
        /// </summary>
        public void Add_Panel2()
        {
            //Указанный пункт назначения, указанный автомобиль, кнопка запуска
            panel2.Controls.Add(button4);
            panel2.Controls.Add(button12);
            panel2.Controls.Add(button15);

            //Добавьте кнопку легенды на панель2
            panel2.Controls.Add(button5);
            panel2.Controls.Add(button6);
            panel2.Controls.Add(button7);
            panel2.Controls.Add(button8);
            panel2.Controls.Add(button9);
            panel2.Controls.Add(button10);
            panel2.Controls.Add(button11);
            panel2.Controls.Add(button1);
            panel2.Controls.Add(button2);


            
            panel2.Controls.Add(label1);
            panel2.Controls.Add(label2);
            panel2.Controls.Add(label3);
            panel2.Controls.Add(label4);
            panel2.Controls.Add(label5);
            panel2.Controls.Add(label6);
            panel2.Controls.Add(label7);
            panel2.Controls.Add(label8);
            panel2.Controls.Add(label9);
        }

        /// <summary>
        /// Установите узел, в котором находится автомобиль, в состояние «занято».
        /// </summary>
        public void VehicleOcuppyNode()
        {
            for (int p = 0; p < Elc.heightNum; p++)
            {
                for (int q = 0; q < Elc.widthNum; q++)
                {
                    Elc.TempMapNode[p, q].nodeCanUsed = true;
                    Elc.mapnode[p, q].nodeCanUsed = true;
                }
            }
            int count = constDefine.VEHICL_COUNT;
            for (int i = 0; i < count; i++)
            {
                Elc.TempMapNode[changeY(vehicle[i].Y), changeX(vehicle[i].X)].nodeCanUsed = false;
                Elc.mapnode[changeY(vehicle[i].Y), changeX(vehicle[i].X)].nodeCanUsed = false;
            }

            //bool a1 = Elc.TempMapNode[5, 5].nodeCanUsed;
            //bool a2 = Elc.TempMapNode[6, 5].nodeCanUsed;
            //bool a3 = Elc.TempMapNode[7, 5].nodeCanUsed;
            //bool a4 = Elc.TempMapNode[8, 5].nodeCanUsed;
            //bool a5 = Elc.TempMapNode[9, 5].nodeCanUsed;
            //bool a6 = Elc.TempMapNode[10, 5].nodeCanUsed;
            //bool a7 = Elc.TempMapNode[11, 5].nodeCanUsed;
            //bool a8 = Elc.TempMapNode[12, 5].nodeCanUsed;
            //bool a9 = Elc.TempMapNode[13, 5].nodeCanUsed;
            //bool a10 = Elc.TempMapNode[14, 5].nodeCanUsed;

        }
        /// <summary>
        /// //Обнаружение конфликтующих узлов и перенаправление
        /// </summary>
        public void CheckeConflictNode()
        {
            //Сортируйте автомобили от мала до велика по стоимости
            SortRoute();
            VehicleOcuppyNode();
            int count = constDefine.VEHICL_COUNT;
            for (int i = 0; i < count - 1; i++)
            {
                Boolean flag = false;
                for (int j = 0; j <= i; j++)
                {
                    if (vehicle[i].vehical_state != v_state.normal || vehicle[j].vehical_state != v_state.normal)
                    {
                        continue;
                    }
                    for (int k = 0; k < vehicle[i + 1].route.Count; k++)
                    {
                        if (k <=vehicle[j].route.Count - 1)
                        {
                            if ((vehicle[j].route[k].x == vehicle[i + 1].route[k].x) && (vehicle[j].route[k].y == vehicle[i + 1].route[k].y))
                            {
                                Elc.TempMapNode[(int)vehicle[j].route[k].x, (int)vehicle[j].route[k].y].nodeCanUsed = false;
                                Elc.mapnode[(int)vehicle[j].route[k].x, (int)vehicle[j].route[k].y].nodeCanUsed = false;
                                flag = true;
                            }
                            //else
                            //{
                            //    Elc.TempMapNode[(int)vehicle[j].route[k].x, (int)vehicle[j].route[k].y].nodeCanUsed = true;
                            //    Elc.mapnode[(int)vehicle[j].route[k].x, (int)vehicle[j].route[k].y].nodeCanUsed = true;
                            //}
                            if (k > 0)
                            {
                                if ((vehicle[j].route[k - 1].x == vehicle[i + 1].route[k].x) && (vehicle[j].route[k - 1].y == vehicle[i + 1].route[k].y))
                                {
                                    Elc.TempMapNode[(int)vehicle[j].route[k].x, (int)vehicle[j].route[k].y].nodeCanUsed = false;
                                    Elc.mapnode[(int)vehicle[j].route[k].x, (int)vehicle[j].route[k].y].nodeCanUsed = false;
                                    flag = true;
                                }
                                //else
                                //{
                                //    Elc.TempMapNode[(int)vehicle[j].route[k].x, (int)vehicle[j].route[k].y].nodeCanUsed = true;
                                //    Elc.mapnode[(int)vehicle[j].route[k].x, (int)vehicle[j].route[k].y].nodeCanUsed = true;
                                //}
                            }
                        }
                        else
                        {
                            if ((vehicle[j].route[vehicle[j].route.Count - 1].y == vehicle[i + 1].route[k].y) && (vehicle[j].route[vehicle[j].route.Count - 1].x == vehicle[i + 1].route[k].x))
                            {
                                Elc.TempMapNode[(int)vehicle[j].route[vehicle[j].route.Count - 1].x, (int)vehicle[j].route[vehicle[j].route.Count - 1].y].nodeCanUsed = false;
                                Elc.mapnode[(int)vehicle[j].route[vehicle[j].route.Count - 1].x, (int)vehicle[j].route[vehicle[j].route.Count - 1].y].nodeCanUsed = false;
                                flag = true;
                            }
                            //else
                            //{
                            //    Elc.TempMapNode[(int)vehicle[j].route[vehicle[j].route.Count - 1].x, (int)vehicle[j].route[vehicle[j].route.Count - 1].y].nodeCanUsed = true;
                            //    Elc.mapnode[(int)vehicle[j].route[vehicle[j].route.Count - 1].x, (int)vehicle[j].route[vehicle[j].route.Count - 1].y].nodeCanUsed = true;
                            //}

                        }
                    }
                    if (flag == true)
                    {
                        vehicle[i + 1].SearchRoute(Elc, vehicle[i + 1].startX, vehicle[i + 1].startY, vehicle[i + 1].endX, vehicle[i + 1].endY);
                    }
                }

            }
        }
        /// <summary>
        /// //Сортируйте автомобили от мала до велика по стоимости
        /// </summary>
        public void SortRoute()
        {
            int count = constDefine.VEHICL_COUNT;
            for (int i = 0; i < count; i++)
            {
                Vehicle temp = vehicle[i];
                for (int j = i; j < count; j++)
                {
                    if (vehicle[i].cost > vehicle[j].cost)
                    {
                        temp = vehicle[j];
                        vehicle[j] = vehicle[i];
                        vehicle[i] = temp;
                    }
                }
                // vehicleSorted[i] = vehicle[index];
            }
        }


        private void Form1_Load(object sender, EventArgs e)
        {
            
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw | ControlStyles.AllPaintingInWmPaint, true);


            StringBuilder sb1 = new StringBuilder("route:");


            for (int w = 0; w < vehicle[1].route.Count; w++)
            {
                //sb1.Append("[" + vehicle[1].route[w].Height + "," + vehicle[1].route[w].Width + "]->");
            }
            //label9.Text = sb1.ToString();


            StringBuilder sb = new StringBuilder();
            int[,] mapstring = new int[Elc.widthNum, Elc.heightNum];
            mapstring = AstarSearch.mapString();

            /*for (int i = 0; i < Elc.heightNum - 1; i++)
            {
                for (int j = 0; j < Elc.widthNum - 1; j++)
                {

                    Boolean flag = false;
                    for (int k = 0; k < vehicle[1].route.Count; k++)
                    {
                        if ((int)vehicle[1].route[k].Height == i && (int)vehicle[1].route[k].Width == j)
                        {
                            sb.Append("* ");
                            flag = true;
                            break;
                        }
                    }
                    if (flag == false)
                    {
                        for (int k = 0; k < vehicle[0].route.Count; k++)
                        {
                            if ((int)vehicle[0].route[k].Height == i && (int)vehicle[0].route[k].Width == j)
                            {
                                sb.Append("# ");
                                flag = true;
                                break;
                            }
                        }
                    }
                    if (flag == false)
                    {
                        if (mapstring[i, j] == 0)
                            sb.Append("0 ");
                        else
                            sb.Append("1 ");
                    }


                }
                sb.Append("\n\t");
            }*/
            //label8.Text = sb.ToString();


        }

        /// <summary>
        /// Действие, вызванное кнопкой запуска
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void StartToolStripMenuItem_Click(object sender, EventArgs e)
        {
            StartToolStripMenuItem.BackColor = System.Drawing.Color.Chartreuse;
            PauseToolStripMenuItem.BackColor = System.Drawing.Color.White;

            Vehicle_Start = true;
            CanStart = true;
        }

        /// <summary>
        /// Действие, вызванное кнопкой остановки
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PauseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            StartToolStripMenuItem.BackColor = System.Drawing.Color.White;
            PauseToolStripMenuItem.BackColor = System.Drawing.Color.Chartreuse;

            Vehicle_Start = false;
            CanStart = false;
        }
        /// <summary>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void enlargeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
            small_number = small_number * 0.95f;

            
            Picture_Length.Push(small_number);

            iWidth = (int)(iWidth / 0.95) + 1;
            Panel_Width.Push(iWidth);

            iHeight = (int)(iHeight / 0.95) + 1;
            Panel_Height.Push(iHeight);


            pic.Image = GetSmall(surface, small_number);

           
            this.panel1.Size = new System.Drawing.Size(iWidth, iHeight);
            this.pic.Size = new System.Drawing.Size(iWidth, iHeight);
        }

        /// <summary>
       
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void reduceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Picture_Length.Count > 1)
            {

                
                Picture_Length.Pop();
                small_number = Picture_Length.Pop();

                Picture_Length.Push(small_number);
                pic.Image = GetSmall(surface, (double)small_number);

                Panel_Width.Pop();
                iWidth = Panel_Width.Pop();
                Panel_Width.Push(iWidth);

                Panel_Height.Pop();
                iHeight = Panel_Height.Pop();
                Panel_Height.Push(iHeight);


                
                this.panel1.Size = new System.Drawing.Size(iWidth, iHeight);
                this.pic.Size = new System.Drawing.Size(iWidth, iHeight);
            }
        }

        /// <summary>
        
        /// </summary>
        /// <param name="bm"></param>
        /// <param name="times"></param>
        /// <returns></returns>
        private Bitmap GetSmall(Bitmap bm, double times)
        {
            int nowWidth = (int)(bm.Width / times);
            int nowHeight = (int)(bm.Height / times);
            Bitmap newbm = new Bitmap(nowWidth, nowHeight);

            if (times >= 1 && times <= 1.1)
            {
                newbm = bm;
            }
            else
            {
                Graphics g = Graphics.FromImage(newbm);
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.SmoothingMode = SmoothingMode.HighQuality;
                g.CompositingQuality = CompositingQuality.HighQuality;
                g.DrawImage(bm, new Rectangle(0, 0, nowWidth, nowHeight), new Rectangle(0, 0, bm.Width, bm.Height), GraphicsUnit.Pixel);
                g.Dispose();
            }
            return newbm;
        }

        public int changeX(int X)
        {
            return (X - constDefine.BEGIN_X) / constDefine.BENCHMARK;
        }
        public int changeY(int Y)
        {
            return Y / constDefine.BENCHMARK;
        }

        /// <summary>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void panel1_Paint(object sender, PaintEventArgs e)
        {
           
        }

        /// <summary>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void panel2_Paint(object sender, PaintEventArgs e)
        {
   
        }

        /// <summary>
        
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pic_MouseClick(object sender, MouseEventArgs e)
        {
            Point point = e.Location;

            //Проверить координаты точки щелчка
            string X = point.X.ToString();
            //string Y = point.Y.ToString();
            //MessageBox.Show(point.ToString(), X + Y);
            int WidthNum, HeightNum;

            string tempstring;

            //Получите индекс положения щелчка мыши и измените тип объекта в исходном изображении.
            WidthNum = point.X / constDefine.BENCHMARK;
            HeightNum = point.Y / constDefine.BENCHMARK;
            if (Object_Change == true)
            {
                string path = "../../XMLFile1.xml";
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(path);
                XmlNode root = xmlDoc.SelectSingleNode("config");//Найдите узел для изменения

                tempstring = "data";
                tempstring = tempstring + (HeightNum + 1).ToString() + "-" + (WidthNum + 1).ToString();

                XmlNode xn = root.SelectSingleNode(tempstring);
                if(xn==null)
                {
                    XmlElement xe = xmlDoc.CreateElement(tempstring);//создать узел
                    xe.InnerText = PicString;
                    root.AppendChild(xe);  
                    xmlDoc.Save("../../XMLFile1.xml");

                    Elc.TempMapNode[HeightNum, WidthNum].oth = SetImage;
                    Elc.TempMapNode[HeightNum, WidthNum].Node_Type = SetType;
                    Elc.mapnode[HeightNum, WidthNum].oth = SetImage;
                    Elc.mapnode[HeightNum, WidthNum].Node_Type = SetType;
                }
                else
                {
                    XmlElement xe = (XmlElement)xn;
                    xe.InnerText = PicString;
                    xmlDoc.Save(path);

                    Elc.TempMapNode[HeightNum, WidthNum].oth = SetImage;
                    Elc.TempMapNode[HeightNum, WidthNum].Node_Type = SetType;
                    Elc.mapnode[HeightNum, WidthNum].oth = SetImage;
                    Elc.mapnode[HeightNum, WidthNum].Node_Type = SetType;
                }   
            }

            //Узнайте, какой автомобиль был нажат
            if (Vehicle_Get == true)
            {   
                int i;
                for ( i= 0; i < constDefine.VEHICL_COUNT; i++)
                {
                    if (vehicle[i].startX == HeightNum && vehicle[i].startY == WidthNum)
                    {
                        Vehicle_Index = i;
                        vehicle[Vehicle_Index].V_Picture = img_Orange;
                        break;
                    }
                }
                if(i<vehicle.Length)
                {
                    Veh = true;
                }
                Vehicle_Get = false;
            }

            //Запишите координаты пункта назначения, на который нажали.
            if (Destination_Get == true&&CanStart==true)
            {
                vehicle[Vehicle_Index].V_Picture = img_Yellow;
                Set_Destination_X = WidthNum;
                Set_Destination_Y = HeightNum;

                if (Elc.TempMapNode[HeightNum, WidthNum].Node_Type == false)
                    MessageBox.Show("Выбранный пункт назначения недоступен», «Внимание! ! !");

                else
                {
                    Elc.TempMapNode[HeightNum, WidthNum].oth = Elc.mapnode[HeightNum, WidthNum].another;

                    vehicle[Vehicle_Index].endX = Set_Destination_Y;
                    vehicle[Vehicle_Index].endY = Set_Destination_X;

                    VehicleOcuppyNode();

                    vehicle[Vehicle_Index].SearchRoute(Elc, vehicle[Vehicle_Index].startX, vehicle[Vehicle_Index].startY, vehicle[Vehicle_Index].endX, vehicle[Vehicle_Index].endY);

                    if (vehicle[Vehicle_Index].Arrive == true)
                    {
                        vehicle[Vehicle_Index].startX = vehicle[Vehicle_Index].endX;
                        vehicle[Vehicle_Index].startY = vehicle[Vehicle_Index].endY;
                    }

                    //for (int i = 0; i < constDefine.VEHICL_COUNT; i++)
                    //{
                    //    VehicleOcuppyNode();
                    //    vehicle[i].SearchRoute(Elc, vehicle[i].startX, vehicle[i].startY, vehicle[i].endX, vehicle[i].endY);
                    //    if (vehicle[i].Arrive == true)
                    //    {
                    //        vehicle[i].startX = vehicle[i].endX;
                    //        vehicle[i].startY = vehicle[i].endY;
                    //    }
                    //}
                }
                
                Destination_Get = false;           
            } 

            //Если нажать кнопку корзины
            if (AGV_Add == true)
            {
                string path = "../../XMLFile1.xml";
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(path);
                XmlNode root = xmlDoc.SelectSingleNode("config");//Найдите узел для изменения

                string Str_H;
                string Str_Z;

                constDefine.VEHICL_COUNT = constDefine.VEHICL_COUNT + 1; 

                Str_H = "agv" + (constDefine.VEHICL_COUNT).ToString() + "-H";
                Str_Z = "agv" + (constDefine.VEHICL_COUNT).ToString() + "-Z";

                XmlElement xe = xmlDoc.CreateElement(Str_H);//создать узел   
                xe.InnerText = WidthNum.ToString();
                root.AppendChild(xe);//добавлено в узел 

                xe = xmlDoc.CreateElement(Str_Z);//создать узел  
                xe.InnerText = HeightNum.ToString();
                root.AppendChild(xe);//добавлено в узел   

                string s = Convert.ToString(constDefine.VEHICL_COUNT);
                XmlNode xn = root.SelectSingleNode("VEHICL_COUNT");
                xe = (XmlElement)xn;
                xe.InnerText = s;
                xmlDoc.Save(path);

                vehicle[constDefine.VEHICL_COUNT-1] = new Vehicle();

                vehicle[constDefine.VEHICL_COUNT - 1] = new Vehicle(HeightNum, WidthNum, vehicle[constDefine.VEHICL_COUNT - 1].img_Vehicle[5], false);

                vehicle[constDefine.VEHICL_COUNT - 1].endX = vehicle[constDefine.VEHICL_COUNT - 1].startX;
                vehicle[constDefine.VEHICL_COUNT - 1].endY = vehicle[constDefine.VEHICL_COUNT - 1].startY;

                //Elc.mapnode[vehicle[constDefine.VEHICL_COUNT - 1].startX, vehicle[constDefine.VEHICL_COUNT - 1].startY].nodeCanUsed = false;
                //Elc.TempMapNode[vehicle[constDefine.VEHICL_COUNT - 1].startX, vehicle[constDefine.VEHICL_COUNT - 1].startY].nodeCanUsed = false;

                Elc.mapnode[HeightNum, WidthNum].nodeCanUsed = false;
                Elc.TempMapNode[HeightNum, WidthNum].nodeCanUsed = false;

                //Установите узел, в котором находится автомобиль, в состояние «занято».
                VehicleOcuppyNode();

                vehicle[constDefine.VEHICL_COUNT - 1].SearchRoute(Elc, vehicle[constDefine.VEHICL_COUNT - 1].startX, vehicle[constDefine.VEHICL_COUNT - 1].startY, vehicle[constDefine.VEHICL_COUNT - 1].endX, vehicle[constDefine.VEHICL_COUNT - 1].endY);

                //Обнаружение конфликтующих узлов и перенаправление
                CheckeConflictNode();
            }
        }

        /// <summary>
       
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button4_Click_1(object sender, EventArgs e)
        {
            //При нажатии кнопки автомобиля кнопка назначения и кнопка замены объекта отключаются.
            Object_Change = false;  
            Destination_Get = false; 
            Vehicle_Get = true;    
            AGV_Add = false;
        }

        /// <summary>
       
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button12_Click(object sender, EventArgs e)
        {
            
            Vehicle_Get = false;
            Object_Change = false;  
            Destination_Get = true;
            AGV_Add = false;
        }

        ///// <summary>
        ///// Прикажите машине переместить все кнопки
        ///// </summary>
        ///// <param name="sender"></param>
        ///// <param name="e"></param>
        //private void button15_Click(object sender, EventArgs e)
        //{
        //    
        //    if (CanStart == true)
        //    {
        //        VehicleOcuppyNode();

        //        for (int i = 0; i < vehicle.Length; i++)
        //        {
        //            vehicle[i].SearchRoute(Elc, vehicle[i].startX, vehicle[i].startY, vehicle[i].endX, vehicle[i].endY);
        //        }

        //        //Обнаружение конфликтующих узлов и перенаправление
        //        CheckeConflictNode();

        //                //Когда автомобиль прибывает в пункт назначения, исходные координаты пункта назначения являются текущими начальными координатами.
        //                for (int i = 0; i < vehicle.Length; i++)
        //                {
        //                    if (vehicle[i].Arrive==true)
        //                    {
        //                        vehicle[i].startX = vehicle[i].endX;
        //                        vehicle[i].startY = vehicle[i].endY;
        //                    }         
        //                }
        //    }
            
        //}
        //Нажмите кнопку корзины, чтобы добавить корзину на карту.
        private void button2_Click(object sender, EventArgs e)
        {
            AGV_Add = true;
            
            Vehicle_Get = false;
            Object_Change = false;  
            Destination_Get = false; 
        }

        //Нажмите на изображение дороги, чтобы изменить изображение и доступность узла.
        private void button5_Click(object sender, EventArgs e)
        {
            
            Vehicle_Get = false;
            Destination_Get = false;
            Object_Change = true; 
            SetImage = img_Road;
            SetType = true;
            PicString = "путь";
            AGV_Add = false;
        }

        //Нажмите кнопку доставки, чтобы изменить изображение и доступность узла.
        private void button8_Click(object sender, EventArgs e)
        {
            //При нажатии кнопки замененного изображения объекта кнопка назначения и кнопка автомобиля недействительны.
            Vehicle_Get = false;
            Destination_Get = false;
            Object_Change = true; 
            SetImage = img_Destination;
            SetType = false;
            PicString = "место доставки";
            AGV_Add = false;
        }

      

        //Нажмите кнопку точки выдачи, чтобы изменить изображение и доступ к узлу.
        private void button7_Click(object sender, EventArgs e)
        {
            
            Vehicle_Get = false;
            Destination_Get = false;
            Object_Change = true;  
            SetImage = img_Belt;
            SetType = true;
            PicString = "место сбора";
            AGV_Add = false;
        }

        //Нажмите кнопку зоны зарядки, чтобы изменить изображение и доступность узла.
        private void button9_Click(object sender, EventArgs e)
        {
            
            Vehicle_Get = false;
            Destination_Get = false;
            Object_Change = true;  
            SetImage = img_ChargeStation;
            SetType = true;
            PicString = "зона зарядки";
            AGV_Add = false;
        }

        //Нажмите кнопку препятствия, чтобы изменить изображение и доступ к узлу.
        private void button10_Click(object sender, EventArgs e)
        {
            
            Vehicle_Get = false;
            Destination_Get = false;
            Object_Change = true;  
            SetImage = img_Obstacle;
            SetType = false;
            PicString = "препятствие";
            AGV_Add = false;
        }

        
        //Нажмите пустую кнопку, чтобы изменить доступ к изображению и узлу.
        private void button1_Click(object sender, EventArgs e)
        {
            
            Vehicle_Get = false;
            Destination_Get = false;
            Object_Change = true;  
            SetImage = img_White;
            SetType = false;
            PicString = "пустой";
            AGV_Add = false;
        }

        /// <summary>
        
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button4_MouseMove(object sender, MouseEventArgs e)
        {
            button4.BackColor = System.Drawing.Color.DeepSkyBlue;
        }


        /// <summary>
       
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button4_MouseLeave(object sender, EventArgs e)
        {
            button4.BackColor = System.Drawing.Color.PowderBlue;
        }

        private void button12_MouseMove(object sender, MouseEventArgs e)
        {
            button12.BackColor = System.Drawing.Color.DeepSkyBlue;
        }

        private void button12_MouseLeave(object sender, EventArgs e)
        {
            button12.BackColor = System.Drawing.Color.PowderBlue;
        }

        private void button15_MouseMove(object sender, MouseEventArgs e)
        {
            button15.BackColor = System.Drawing.Color.DeepSkyBlue;
        }

        private void button15_MouseLeave(object sender, EventArgs e)
        {
            button15.BackColor = System.Drawing.Color.PowderBlue;
        }

        /// <summary>
        /// Показать кнопку пути движения автомобиля
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ShowVehicleRouteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
            F_VehicleRoute.Icon = ((System.Drawing.Icon)(Resources.youzheng));

           
            F_VehicleRoute.Size = new System.Drawing.Size(500, 500);
            L_VehicleRoute.Size = new System.Drawing.Size(480, 480);

            //StringBuilder sb1 = new StringBuilder("route:");
            StringBuilder[] sb1 = new StringBuilder[constDefine.VEHICL_COUNT];

            StringBuilder Together = new StringBuilder();

            for (int i = 0; i < constDefine.VEHICL_COUNT; i++)
            {
                sb1[i] = new StringBuilder("Путь автомобиля "+(i+1)+":");
            }


            for (int i = 0; i < constDefine.VEHICL_COUNT;i++ )
            {
                for (int w = 0; w < vehicle[i].route.Count; w++)
                {
                    if(w>=1)
                    {
                        sb1[i].Append("[" + vehicle[i].route[w].Y + "," + vehicle[i].route[w].X + "]");
                        if(w < vehicle[i].route.Count - 1)
                        {
                            sb1[i].Append("->");
                        }
                       
                    }                   
                }
                Together.Append(sb1[i]+"\n\t");
            }
            L_VehicleRoute.Text = Together.ToString();

          
            F_VehicleRoute.Controls.Add(L_VehicleRoute);

            F_VehicleRoute.ShowDialog();    
        }

        /// <summary>
       
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void setHWToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
            F_HegWethBech.Icon = ((System.Drawing.Icon)(Resources.youzheng));

            F_HegWethBech.Size = new System.Drawing.Size(350,350); 

            ///////////////////////////вершина////////////////////////////
            P_TopPanel.Location = new System.Drawing.Point(10,10);    
            this.P_TopPanel.Size = new System.Drawing.Size(340, 30); 
            B_HWForm.Location = new System.Drawing.Point(0,0);
            B_HWForm.BackColor = System.Drawing.Color.Chartreuse;
            B_HWForm.Text = "Длина и ширина рамы";
            B_HWMap.Location = new System.Drawing.Point(80,0);
            B_HWMap.Text = "Длина и ширина карты";
            B_HWMap.BackColor = System.Drawing.Color.PowderBlue;

            B_HWForm.Click += new System.EventHandler(this.B_HWForm_Click);
            B_HWMap.Click += new System.EventHandler(this.B_HWMap_Click);

            P_TopPanel.Controls.Add(B_HWForm);
            P_TopPanel.Controls.Add(B_HWMap);

            ///////////////////////////дно///////////////////////////
            P_BottomPanel.Location = new System.Drawing.Point(0,210);
            P_BottomPanel.Size = new System.Drawing.Size(350,140);

            B_OKBotton.Location = new System.Drawing.Point(75,10);
            B_CANCELBotton.Location = new System.Drawing.Point(175, 10);

            B_OKBotton.Text = "подтверждать";
            B_CANCELBotton.Text = "Отмена";

            //B_OKBotton.Click += new System.EventHandler(this.B_OKBotton_Click);
            B_CANCELBotton.Click += new System.EventHandler(this.B_CANCELBotton_Click);

            P_BottomPanel.Controls.Add(B_OKBotton);
            P_BottomPanel.Controls.Add(B_CANCELBotton);

            //////////////////////////////////////////////////////
            P_HWForm.Location = new System.Drawing.Point(0,40);
            P_HWForm.Size = new System.Drawing.Size(350,170);

            
            L_HegForm.Text = "длина кадра";
            L_WethForm.Text = "ширина рамы";
            L_Bech.Text = "ориентир";

            //позиция метки
            this.L_HegForm.Location = new System.Drawing.Point(60, 65);
            this.L_WethForm.Location = new System.Drawing.Point(60, 105);
            this.L_Bech.Location = new System.Drawing.Point(60, 125);



            //Положение, длина и ширина поля ввода
            T_HegForm.SetBounds(160, 60, 90, 60);
            T_WethForm.SetBounds(160, 100, 90, 60);
            T_Bech.SetBounds(120, 120, 90, 60);

            P_HWForm.Controls.Add(T_HegForm);
            P_HWForm.Controls.Add(T_WethForm);
            P_HWForm.Controls.Add(L_HegForm);
            P_HWForm.Controls.Add(L_WethForm);

//////////////////////////////////////////////Длина и ширина карты///////////////////////////////////////////
            P_HWMap.Location = new System.Drawing.Point(0,40);
            P_HWMap.Size = new System.Drawing.Size(350,170);

            L_HegMap.Text = "длина карты";
            L_WethMap.Text = "ширина карты";

 
            
            this.L_HegMap.Location = new System.Drawing.Point(60, 65);
            this.L_WethMap.Location = new System.Drawing.Point(60, 105);
           
           
            T_HegMap.SetBounds(160, 60, 90, 60);
            T_WethMap.SetBounds(160, 100, 90, 60);
            T_Bech.SetBounds(120, 120, 90, 60);

            P_HWMap.Controls.Add(T_HegMap);
            P_HWMap.Controls.Add(T_WethMap);
            P_HWMap.Controls.Add(L_HegMap);
            P_HWMap.Controls.Add(L_WethMap);
            
////////////////////////////////////////////////////////////////////////////////////////////////////////
            F_HegWethBech.Controls.Add(P_TopPanel);
            F_HegWethBech.Controls.Add(P_HWForm);
            F_HegWethBech.Controls.Add(P_BottomPanel);

            F_HegWethBech.ShowDialog();
        }

        /// <summary>
       
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void B_HWForm_Click(object sender, EventArgs e)
        {
            B_HWForm.BackColor = System.Drawing.Color.Chartreuse;
            B_HWMap.BackColor = System.Drawing.Color.PowderBlue;
            F_HegWethBech.Controls.Remove(P_HWMap);
            F_HegWethBech.Controls.Add(P_HWForm);
        }

        /// <summary>
       
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void B_HWMap_Click(object sender, EventArgs e)
        {
            B_HWForm.BackColor = System.Drawing.Color.PowderBlue;
            B_HWMap.BackColor = System.Drawing.Color.Chartreuse;
            F_HegWethBech.Controls.Remove(P_HWForm);
            F_HegWethBech.Controls.Add(P_HWMap);
        }

        /// <summary>
      
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void B_OKBotton_Click(object sender, EventArgs e)
        {

            string path = "../../XMLFile1.xml";
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(path);
            XmlNode root = xmlDoc.SelectSingleNode("config");//Найдите узел для изменения

///////////////////////////////////////фреймворк///////////////////////////////////////////

            
            int Out_HeighFormt;
            int Out_WidthForm;
            int Out_Bech;

            int.TryParse(T_HegForm.Text, out Out_HeighFormt);
            int.TryParse(T_HegForm.Text, out Out_WidthForm);
            int.TryParse(T_HegForm.Text, out Out_Bech);

           
            if (String.IsNullOrEmpty(T_HegForm.Text))
            {
            }
            else
            {
                this.Height = Out_HeighFormt;

                
                string s = Convert.ToString(this.Height);
                XmlNode xn = root.SelectSingleNode("Form_Height");
                XmlElement xe = (XmlElement)xn;
                xe.InnerText = s;
                xmlDoc.Save(path);
            }

            if (String.IsNullOrEmpty(T_WethForm.Text))
            {
            }
            else
            {
                
                this.Width = Out_WidthForm;
                string s = Convert.ToString(this.Width);
                XmlNode xn = root.SelectSingleNode("Form_Width");
                XmlElement xe = (XmlElement)xn;
                xe.InnerText = s;
                xmlDoc.Save(path);
            }


            if (String.IsNullOrEmpty(T_Bech.Text))
            {
            }
            else
                constDefine.BENCHMARK = Out_Bech;

////////////////////////////////////////карта////////////////////////////////////////////////

            //Преобразуйте строку в поле ввода в число, а затем присвойте ей длину, ширину и основание.
            int Out_HeightMap;
            int Out_WeithMap;

            int.TryParse(T_HegMap.Text, out Out_HeightMap);
            int.TryParse(T_WethMap.Text, out Out_WeithMap);
            int.TryParse(T_Bech.Text, out Out_Bech);

            //Если ввод длины, ширины или базы данных пуст, ничего с ним не делайте.
            if (String.IsNullOrEmpty(T_HegMap.Text))
            {
            }
            else
            {
                this.Height = Out_HeightMap;

                //Передайте значение длины поля, установленное пользователем в тексте, в файл конфигурации XML.
                string s = Convert.ToString(this.Height);
                XmlNode xn = root.SelectSingleNode("HEIGHT");
                XmlElement xe = (XmlElement)xn;
                xe.InnerText = s;
                xmlDoc.Save(path);
            }

            if (String.IsNullOrEmpty(T_WethMap.Text))
            {
            }
            else
            {
               
                this.Width = Out_WeithMap;
                string s = Convert.ToString(this.Width);
                XmlNode xn = root.SelectSingleNode("WIDTH");
                XmlElement xe = (XmlElement)xn;
                xe.InnerText = s;
                xmlDoc.Save(path);
            }

            
            //T_HegMap.Clear();
            //T_WethMap.Clear();
            //T_HegForm.Clear();
            //T_WethForm.Clear();

            
            F_HegWethBech.Dispose();

            this.Dispose();
            //System.Diagnostics.Process.Start(System.Reflection.Assembly.GetExecutingAssembly().Location);
         
            Application.Restart();
        }

        /// <summary>
       
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void B_CANCELBotton_Click(object sender, EventArgs e)
        {
            
            T_HegMap.Clear();
            T_WethMap.Clear();
            T_HegForm.Clear();
            T_WethForm.Clear();
            
            F_HegWethBech.Close();
        }

        private void label6_Click(object sender, EventArgs e)
        {

        }
        private void pic_Click(object sender, EventArgs e)
        {

        }
        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }
        //private GMapOverlay objects = new GMapOverlay("objects");   

        //private void gMapControl1_Load_1(object sender, EventArgs e)
        //{
        //    gMapControl1.Manager.Mode = AccessMode.CacheOnly;
        //    MessageBox.Show("No internet connection avaible, going to CacheOnly mode.", "GMap.NET Demo", MessageBoxButtons.OK, MessageBoxIcon.Warning);

        //    gMapControl1.CacheLocation = Environment.CurrentDirectory + "\\GMapCache\\"; 
        //    gMapControl1.MapProvider = GMapProviders.GoogleChinaMap; 
        //    gMapControl1.MinZoom = 2;  
        //    gMapControl1.MaxZoom = 17; 
        //    gMapControl1.Zoom = 5;     
        //    gMapControl1.ShowCenter = false; 
        //    gMapControl1.DragButton = System.Windows.Forms.MouseButtons.Left; 
        //    gMapControl1.Position = new PointLatLng(32.064, 118.704); 

        //    gMapControl1.Overlays.Add(objects);

        //}
    }    
}
