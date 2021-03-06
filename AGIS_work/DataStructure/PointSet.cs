﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace AGIS_work.DataStructure
{
    /// <summary>
    /// 数据点集合
    /// </summary>
    public class PointSet
    {
        public string SetName { get; private set; }
        public string FileName { get; private set; }

        public List<DataPoint> PointList { get; private set; }
        public MinBoundRect MBR { get; private set; }

        public PointSet() { MBR = new MinBoundRect(-1, -1, 1, 1); PointList = new List<DataPoint>(); }
        public PointSet(string setname, string filename, DataPoint[] points)
        {
            this.SetName = setname;
            this.FileName = filename;
            this.PointList = new List<DataPoint>(points);
            //最小外接矩形
            MBR = new MinBoundRect();
            foreach (DataPoint point in points)
                MBR.UpdateRect(point.X, point.Y);
        }

        /// <summary>
        /// 从CSV文件中读取点集
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        public static PointSet ReadFromCSV(string filename)
        {
            PointSet pointSet = null;
            StreamReader sr = new StreamReader(filename);
            List<DataPoint> dataPoints = new List<DataPoint>();
            try
            {
                string setName = sr.ReadLine();
                int oid = 0;
                while (!sr.EndOfStream)
                {
                    string onePoint = sr.ReadLine();
                    string[] pointInfo = onePoint.Split(',');
                    dataPoints.Add(new DataPoint(int.Parse(pointInfo[0]), pointInfo[1], double.Parse(pointInfo[2]),
                         double.Parse(pointInfo[3]), double.Parse(pointInfo[4])));
                    oid++;
                }
                pointSet = new PointSet(setName, filename, dataPoints.ToArray());
            }
            catch (Exception err) { throw err; }
            sr.Close();
            return pointSet;
        }

        /// <summary>
        /// 将点集写入CSV文件
        /// </summary>
        /// <param name="filename"></param>
        public void WriteToCSV(string filename = null)
        {
            string filePath = filename == null ? this.FileName : filename;
            StreamWriter sw = new StreamWriter(filePath);
            try
            {
                sw.WriteLine(this.SetName);
                foreach (DataPoint point in this.PointList)
                {
                    sw.WriteLine(string.Format("{0},{1},{2],{3},{4}", point.ID, point.Name, point.X, point.Y, point.Value));
                }

            }
            catch (Exception err)
            {
                throw err;
            }
            sw.Close();
            return;
        }

        /// <summary>
        /// 根据OID返回数据点
        /// </summary>
        /// <param name="oid"></param>
        /// <returns></returns>
        public DataPoint GetPointByOID(int oid)
        {
            foreach (var point in PointList)
            { if (point.OID == oid) return point; }
            return null;
        }

        /// <summary>
        /// 添加数据点（OID不重复）
        /// </summary>
        /// <param name="point"></param>
        /// <returns>是否添加成功</returns>
        public bool AddPoint(DataPoint point)
        {
            if (GetPointByOID(point.OID) == null)
            { PointList.Add(point); return true; }
            else return false;
        }

        /// <summary>
        /// 返回全部数据点的OID
        /// </summary>
        /// <returns></returns>
        public List<int> GetPointOIDList()
        {
            List<int> OIDList = new List<int>();
            foreach (var point in PointList)
                OIDList.Add(point.OID);
            return OIDList;
        }
    }
}
