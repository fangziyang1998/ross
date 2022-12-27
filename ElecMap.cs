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

namespace AGV_V1._0
{
    class ElecMap
    {
        public Image img_Belt = Resources.Belt;
        public Image img_Mid = Resources.Mid;
        public Image img_Road = Resources.Road;
        public Image img_Destination = Resources.Destination;
        public Image img_ChargeStation = Resources.ChargeStation;
        public Image img_Obstacle = Resources.Obstacle;
        public Image img_Scanner = Resources.Scanner;
        public Image img_Display = Resources.Display;


        //Ширина электронной карты
        public int Width
        {
            get;
            set;
        }

        //Длина электронной карты
        public int Height
        {
            get;
            set;
        }

        //Количество длинных делений электронной карты
        public int heightNum
        {
            get;
            set;
        }

        //Количество делений ширины электронной карты
        public int widthNum
        {
            get;
            set;
        }
        //private MapNode[,] mapNode;

        public MapNode[,] mapnode;

        public MapNode[,] TempMapNode;

        public String[,] str;
        

        /// <summary>
        /// Конструктор, инициализируйте длину и ширину электронной карты и опорную длину каждого маленького блока
        /// </summary>
        /// <param name="width">Ширина электронной карты</param>
        /// <param name="height">Длина электронной карты</param>
        /// <param name="benchmark">Базовая длина каждого маленького блока</param>

        public ElecMap(int height, int width )
        {
            this.Width = width;
            this.Height = height;
        }

        
        public ElecMap() { }

        public void Draw_Node(Graphics g)
        {
            //Graphics g = e.Graphics;
            int point_x = constDefine.BEGIN_X;
            int point_y = 0;
            int i, j;

            for (i = 0; i < heightNum; i++)
            {
                point_x = constDefine.BEGIN_X;
                for (j = 0; j < widthNum; j++)
                {
                    //Elc.mapnode[i, j] = new MapNode(point_x, point_y, Node_number, point_type);
                    TempMapNode[i, j].x = point_x;
                    TempMapNode[i, j].y = point_y;
                    point_x += constDefine.BENCHMARK;
                }
                point_y += constDefine.BENCHMARK;
            }
        }

        /// <summary>
        
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public void  Map(string name,MapNode node)
        {
            if (name == "путь")
            {
                node.oth = img_Road;
                node.Node_Type = true;
            }
            if (name == "препятствие")
            {
                node.oth = img_Obstacle;
                node.Node_Type = false;
            }
            if (name == "зона зарядки")
            {
                node.oth = img_ChargeStation;
                node.Node_Type = true;
            }
            if (name == "Место сбора")
            {
                node.oth = img_Belt;
                node.Node_Type = true;
            }
           
            if (name == "место доставки")
            {
                node.oth=img_Destination ;
                node.Node_Type=false;
            }
            if (name == "через дорогу")
            {
                node.oth = img_Mid;
                node.Node_Type = true;
            }
        }

        /// <summary>
        /// Инициализировать электронную карту, присвоить все атрибуты объекта и одновременно расположить объекты на электронной карте
        /// </summary>
        /// <param name="g"></param>

        public void SetObject()
        {
            string path = "../../XMLFile1.xml";

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(path);

            //Получить корневой узел
            XmlNode root = xmlDoc.SelectSingleNode("config");

            int Node_number = 1;    

            //Количество разделенной длины и ширины электронной карты
            heightNum = Height / constDefine.BENCHMARK;
            widthNum = Width / constDefine.BENCHMARK;

            mapnode = new MapNode[heightNum,widthNum];
            TempMapNode =new MapNode [heightNum,widthNum];  
            str=new String[heightNum,widthNum];

           
            int i = 0;
            int j = 0;

            
            int point_x, point_y;

           
            bool point_type = false;

            point_x = constDefine.BEGIN_X;
            point_y = 0;

            XmlNode xn;
            string Str_Temp;
            string Str_Con;

            for (i = 0; i < heightNum; i++)
            {
                for (j = 0; j < widthNum; j++)
                {
                    Str_Temp = "data";
                    Str_Temp = Str_Temp + (i+1).ToString() +"-"+(j+1).ToString();
                    str[i, j] = Str_Temp;
                }
            }
                for (i = 0; i < heightNum; i++)
                {
                    point_x = constDefine.BEGIN_X;
                    for (j = 0; j < widthNum; j++)
                    {
                        mapnode[i, j] = new MapNode(point_x, point_y, Node_number, point_type);
                        TempMapNode[i, j] = new MapNode(point_x, point_y, Node_number, point_type);
                        mapnode[i, j].another = img_Display;
                        TempMapNode[i, j].another = img_Display;
                        Node_number++;
                        point_x += constDefine.BENCHMARK;
                    }
                    point_y += constDefine.BENCHMARK;
                }
            
            
            for (i = 0; i < heightNum; i++)
            {
                point_x = constDefine.BLANK_X;
                for (j = 0; j < widthNum; j++)
                {
                    xn = root.SelectSingleNode(str[i,j]);
                    if (xn == null)
                        break;
                    Str_Con = Convert.ToString(xn.InnerText);
                    Map(Str_Con, mapnode[i, j]);
                    Map(Str_Con, TempMapNode[i, j]);
                }
            }
        }

        /// <summary>
        
        /// </summary>
        /// <param name="SourceImage"></param>
        /// <param name="TargetWidth"></param>
        /// <param name="TargetHeight"></param>
        /// <returns></returns>
        public Image ChargePicture(Image SourceImage, int TargetWidth, int TargetHeight)
        {
            int IntWidth; 
            int IntHeight; 
            try
            {
                System.Drawing.Imaging.ImageFormat format = SourceImage.RawFormat;
                System.Drawing.Bitmap SaveImage = new System.Drawing.Bitmap(TargetWidth,TargetHeight);
                Graphics g = Graphics.FromImage(SaveImage);
                g.Clear(Color.White);

                

                if (SourceImage.Width > TargetWidth && SourceImage.Height <=TargetHeight)
                {
                    IntWidth = TargetWidth;
                    IntHeight = (IntWidth * SourceImage.Height) / SourceImage.Width;
                }
                else if (SourceImage.Width <= TargetWidth && SourceImage.Height >TargetHeight)
                {
                    IntHeight = TargetHeight;
                    IntWidth = (IntHeight * SourceImage.Width) / SourceImage.Height;
                }
                else if (SourceImage.Width <= TargetWidth && SourceImage.Height <=TargetHeight) 
                {
                    IntHeight = TargetWidth;
                    IntWidth = TargetHeight;
                }
                else
                {
                    IntWidth = TargetWidth;
                    IntHeight = (IntWidth * SourceImage.Height) / SourceImage.Width;
                    if (IntHeight > TargetHeight)
                    {
                        IntHeight = TargetHeight;
                        IntWidth = (IntHeight * SourceImage.Width) / SourceImage.Height;
                    }
                }

                g.DrawImage(SourceImage, (TargetWidth - IntWidth) / 2, (TargetHeight -IntHeight) / 2, IntWidth, IntHeight);
                //SourceImage.Dispose();

                return SaveImage;
            }
            catch (Exception ex)
            {

            }
            return null;
        }
        /// <summary>
        
        /// </summary>
        ~ElecMap()
        {
        }

    }
}
