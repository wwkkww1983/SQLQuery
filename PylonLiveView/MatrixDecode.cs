﻿using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Win32;
using System.Runtime.InteropServices;
using Euresys.Open_eVision_1_2;
using System.Drawing;
using System.IO;
using System.Drawing.Imaging;
using System.Threading;

namespace PylonLiveView
{
    //[ClassInterface(ClassInterfaceType.None)]
    public class MatrixDecode
    {
        #region 条码解析模块
        /// <summary>
        /// 从图片中获得DecodeString
        /// </summary>
        /// <param name="imagePath"></param>
        /// <returns></returns>
        public string GetDecodeStrbyPath(string imagePath)
        {
             try
            {
                //return "1234567890"; //fortest
                if ((imagePath.Length == 0) || (!File.Exists(imagePath)))
                { return ""; }
                EMatrixCodeReader EMatrixCodeReader1 = new EMatrixCodeReader(); // EMatrixCodeReader instance
                EMatrixCode EMatrixCodeReader1Result = null; // EMatrixCode               
                EImageBW8 EBW8Image1 = new EImageBW8(); // EImageBW8 instance
                EBW8Image1.Load(imagePath);
                EMatrixCodeReader1.TimeOut = GlobalVar.gl_decode_timeout;
                //EMatrixCodeReader1Result = EMatrixCodeReader1.Read(EBW8Image1);
                ////Global.GlobalVar.gl_str_decode[DeviceID] = EMatrixCodeReader1Result.DecodedString;
                //return EMatrixCodeReader1Result.DecodedString;
                return GetDecodeStrbyEImageBW8(EBW8Image1); //ltt
            }
            catch
            {
                return "";
            }
        }

        /// <summary>
        /// 从EImageBW8中获得DecodeString
        /// </summary>
        /// <param name="EImageBW8"></param>
        /// <param name="Redecode">是否需要重复解析</param>
        /// <returns></returns>
        public string GetDecodeStrbyEImageBW8(EImageBW8 EBW8Image1)
        {
            EImageBW8 image_bak = new EImageBW8(EBW8Image1);
            try
            {
                //return "1234567890"; //fortest
                float center_x = image_bak.Width / 2;
                float center_y = image_bak.Height / 2;
                for (int i = 0; i <= GlobalVar.gl_decode_times; i++)
                {
                    if (GlobalVar.gl_inEmergence) { return ""; }
                    EMatrixCodeReader EMatrixCodeReader1 = new EMatrixCodeReader(); // EMatrixCodeReader instance
                    EMatrixCode EMatrixCodeReader1Result = null; // EMatrixCode instance
                    try
                    {
                        EMatrixCodeReader1.TimeOut = GlobalVar.gl_decode_timeout;
                        #region 对图片进行其他处理 ----没有经过验证，无用

                        //定义数组保存位图
                        //int bytes = Math.Abs(bmpdata_src.Stride) * bmp.Height;
                        //byte[] rgbvalues = new byte[bytes];
                        ////复制RGB值到数组
                        //System.Runtime.InteropServices.Marshal.Copy(pScan0, rgbvalues, 0, bytes);
                        //将每个像素的第三个值设为255. A 24bpp的位图将变红
                        //for (int counter = 2; counter < rgbvalues.Length; counter += 3)
                        //{
                        //    rgbvalues[counter] = 255;
                        //}
                        //把RGB值拷回位图
                        //System.Runtime.InteropServices.Marshal.Copy(rgbvalues, 0, ptr, bytes);
                        //解锁
                        //bmp.UnlockBits(bmpdata_src);
                        //绘制更新了的位图
                        //DrawImage(bmp, 0, 150);

                        #endregion

                        EMatrixCodeReader1Result = EMatrixCodeReader1.Read(image_bak);
                        return EMatrixCodeReader1Result.DecodedString;
                    }
                    catch
                    {
                        switch (i)
                        {
                            case 0:
                                //EasyImage.ScaleRotate(image_bak, center_x, center_y, center_x, center_y, 1.23f, 1.23f, 20f, image_bak);
                                image_bak = EImageBW8GainOff(EBW8Image1, float.Parse("0.643"), float.Parse("-69.0"));
                                break;
                            case 1:
                                EasyImage.ScaleRotate(image_bak, center_x, center_y, center_x, center_y, 1.23f, 1.23f, 90f, image_bak);
                                image_bak = EImageBW8GainOff(EBW8Image1, float.Parse("0.643"), float.Parse("-29.0"));
                                break;
                            case 2:
                                //EasyImage.ScaleRotate(image_bak, center_x, center_y, center_x, center_y, 1.23f, 1.23f, 40f, image_bak);
                                image_bak = EImageBW8GainOff(EBW8Image1, float.Parse("1.243"), float.Parse("-89.0"));
                                break;
                            case 3:
                                //EasyImage.ScaleRotate(image_bak, center_x, center_y, center_x, center_y, 1.23f, 1.23f, 50f, image_bak);
                                image_bak = EImageBW8GainOff(EBW8Image1, float.Parse("1.243"), float.Parse("0.0"));
                                break;
                            case 4:
                                //EasyImage.ScaleRotate(image_bak, center_x, center_y, center_x, center_y, 1.23f, 1.23f, 180f, image_bak);
                                image_bak = EImageBW8GainOff(EBW8Image1, float.Parse("1.30"), float.Parse("0.0"));
                                image_bak = EImageBW8GainOff(EBW8Image1, float.Parse("0.0"), float.Parse("48.0"));
                                break;
                            case 5:
                                EasyImage.ScaleRotate(image_bak, center_x, center_y, center_x, center_y, 1.23f, 1.23f, 70f, image_bak);
                                image_bak = EImageBW8GainOff(EBW8Image1, float.Parse("0.643"), float.Parse("0.0"));
                                image_bak = EImageBW8GainOff(EBW8Image1, float.Parse("0.0"), float.Parse("118.0"));
                                break;
                            case 6:
                                EasyImage.ScaleRotate(image_bak, center_x, center_y, center_x, center_y, 1.23f, 1.23f, 80f, image_bak);
                                image_bak = EImageBW8GainOff(EBW8Image1, float.Parse("0.443"), float.Parse("0.0"));
                                image_bak = EImageBW8GainOff(EBW8Image1, float.Parse("0.0"), float.Parse("129.0"));
                                break;
                        }
                    }
                }
                return "";
            }
            catch
            {
                return "";
            }
            finally
            {
                EBW8Image1 = null;
                image_bak = null;
                GC.Collect();
            }
        }

        public EImageBW8 ConvertBitmapToEImageBW8(Bitmap bmp)
        {
            try
            {
                EImageBW8 EBW8Image1 = new EImageBW8(bmp.Width, bmp.Height); // EImageBW8 instance
                //EImageC24 eimageC24 = new EImageC24(bmp.Width, bmp.Height); // EImageC24 instance

                Rectangle rect = new Rectangle(0, 0, bmp.Width, bmp.Height);
                //锁定位图
                System.Drawing.Imaging.BitmapData bmpdata_src = bmp.LockBits(rect, System.Drawing.Imaging.ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);
                //获取首行地址
                IntPtr pScan0 = bmpdata_src.Scan0;
                unsafe
                {
                    try
                    {
                        for (int Height = 0; Height < bmpdata_src.Height; Height++)
                        {
                            byte* pSrc = (byte*)pScan0;
                            pSrc += bmpdata_src.Stride * Height;
                            byte* pDest = (byte*)EBW8Image1.GetImagePtr(0, Height);
                            for (int Width = 0; Width < bmpdata_src.Width; Width++)
                            {
                                pDest[0] = (byte)(pSrc[0] * 0.3 + pSrc[1] * 0.6 + pSrc[2] * 0.1);
                                pSrc += 3;
                                pDest++;
                            }
                        }
                    }
                    catch { }
                }
                bmp.UnlockBits(bmpdata_src);
                return EBW8Image1;
            }
            catch
            {
                return new EImageBW8(640, 480);
            }
        }

        public EImageC24 ConvertBitmapToEImageC24(Bitmap bmp)
        {
            try
            {
                EImageC24 eimageC24 = new EImageC24(bmp.Width, bmp.Height); // EImageC24 instance
                Rectangle rect = new Rectangle(0, 0, bmp.Width, bmp.Height);
                //锁定位图
                System.Drawing.Imaging.BitmapData bmpdata_src = bmp.LockBits(rect, System.Drawing.Imaging.ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);
                //获取首行地址
                IntPtr pScan0 = bmpdata_src.Scan0;
                unsafe
                {
                    try
                    {
                        for (int Height = 0; Height < bmpdata_src.Height; Height++)
                        {
                            byte* pSrc = (byte*)pScan0;
                            pSrc += bmpdata_src.Stride * Height;
                            byte* pDest = (byte*)eimageC24.GetImagePtr(0, Height);
                            for (int Width = 0; Width < bmpdata_src.Width; Width++)
                            {
                                pDest[0] = (byte)(pSrc[0]);
                                pDest[1] = (byte)(pSrc[1]);
                                pDest[2] = (byte)(pSrc[2]);
                                pSrc += 3;
                                pDest += 3;
                            }
                        }
                    }
                    catch { }
                }
                bmp.UnlockBits(bmpdata_src);
                return eimageC24;
            }
            catch { return new EImageC24(640, 480); }
        }
        
        /// <summary>
        /// 图片均衡操作，将传入图片自动转换
        /// 图片(forexample:a.bmp)另存为同文件夹下面的_a.bmp
        /// </summary>
        /// <param name="ImagePath"></param>
        public void QualizerImage(string imagePath)
        {
            try
            {
                if ((imagePath.Length == 0) || (!File.Exists(imagePath)))
                { return; }
                EImageBW8 EBW8ImageOrig = new EImageBW8(); // EImageBW8 instance
                EImageBW8 EBW8ImageDest = new EImageBW8(); // EImageBW8 instance
                EBW8ImageOrig.SetSize(512, 512);
                // Make image black
                EasyImage.Oper(EArithmeticLogicOperation.Copy, new EBW8(0), EBW8ImageOrig);
                EBW8ImageOrig.Load(imagePath);
                EBW8ImageDest.SetSize(512, 512);
                // Make image black
                EasyImage.Oper(EArithmeticLogicOperation.Copy, new EBW8(0), EBW8ImageDest);
                EBW8ImageDest.SetSize(EBW8ImageOrig);
                EasyImage.Equalize(EBW8ImageOrig, EBW8ImageDest);
                //EBW8ImageOrig.Dispose();
                EBW8ImageDest.Save(ImageSaveAsPath(imagePath));
            }
            catch
            {}
        }

        /// <summary>
        /// 图片均衡操作，将传入图片自动转换，imgWidth：设定图片宽度，imgHeight图片设定高度
        /// 图片(forexample:a.bmp)另存为同文件夹下面的_a.bmp
        /// </summary>
        /// <param name="ImagePath"></param>
        public void QualizerImage(string ImagePath, int imgWidth, int imgHeight)
        {
            try
            {
                if ((ImagePath.Length == 0) || (!File.Exists(ImagePath)))
                { return; }
                EImageBW8 EBW8ImageOrig = new EImageBW8(); // EImageBW8 instance
                EImageBW8 EBW8ImageDest = new EImageBW8(); // EImageBW8 instance
                EBW8ImageOrig.SetSize(imgWidth, imgHeight);
                // Make image black
                EasyImage.Oper(EArithmeticLogicOperation.Copy, new EBW8(0), EBW8ImageOrig);
                EBW8ImageOrig.Load(ImagePath);
                EBW8ImageDest.SetSize(imgWidth, imgHeight);
                // Make image black
                EasyImage.Oper(EArithmeticLogicOperation.Copy, new EBW8(0), EBW8ImageDest);
                EBW8ImageDest.SetSize(EBW8ImageOrig);
                EasyImage.Equalize(EBW8ImageOrig, EBW8ImageDest);
                EBW8ImageOrig.Dispose();
                //EBW8ImageDest.Save(ImagePath);
                EBW8ImageDest.Save(ImageSaveAsPath(ImagePath));
            }
            catch
            { }
        }

        /// <summary>
        /// 灰度&对比度处理，图片另存为imagePath\_*.*
        /// 图片(forexample:a.bmp)另存为同文件夹下面的_a.bmp
        /// </summary>
        /// <param name="ImagePath"></param>
        /// <param name="Val_gain"></param>
        /// <param name="Val_offSet"></param>
        public EImageBW8 EImageBW8GainOff(EImageBW8 ImageSource, float Val_gain, float Val_offSet)
        {
            EImageBW8 EBW8ImageDest = new EImageBW8(ImageSource.Width, ImageSource.Height); // EImageBW8 instance
            try
            {
                EasyImage.GainOffset(ImageSource, EBW8ImageDest, Val_gain, Val_offSet);
            }
            catch{ }
            return EBW8ImageDest;
        }

        private string ImageSaveAsPath(string ImagePath)
        {
            try
            {
                string Folderpath = ImagePath.Substring(0, ImagePath.LastIndexOf('\\') + 1);
                string ImageName = ImagePath.Substring(ImagePath.LastIndexOf('\\') + 1);
                string resultPath = Folderpath + "_" + ImageName;
                return resultPath;
            }
            catch
            {
                throw;
            }
        }
        #endregion

        #region 形状分析模块
        public List<PositionInfo> ShapeMatch(Bitmap bmp)
        {
            List<PositionInfo> _Resultlist = new List<PositionInfo>();
            try
            {
                string MCH_matchFile = GlobalVar.gl_matchFileName;
                EMatcher EMatch1 = new EMatcher();
                //單反相機需要用ec24
                EImageBW8 bw8image = ConvertBitmapToEImageBW8(bmp);
                EMatch1.Load(MCH_matchFile);

                EMatch1.MaxPositions = 10;
                EMatch1.MinScore = GlobalVar.gl_MinMatchScore * 1.00f / 100;  
                EMatch1.Match(bw8image);
                for (int i = 0; i < EMatch1.NumPositions; i++)
                {
                    PositionInfo info = new PositionInfo();
                    EMatchPosition pos = EMatch1.GetPosition(i);
                    info.CenterX = pos.CenterX;
                    info.CenterY = pos.CenterY;
                    info.angle = pos.Angle;
                    info.scaleX = pos.ScaleX;
                    info.scaleY = pos.ScaleY;
                    info.score = pos.Score;
                    info.MCHPatterWidth = EMatch1.PatternWidth;
                    info.MCHPatterHeight = EMatch1.PatternHeight;
                    _Resultlist.Add(info);
                }
                return _Resultlist;
            }
            catch (EException)
            {
                return new List<PositionInfo>();
            }
        }
        #endregion
    }

    public class PositionInfo
    {
        public float CenterX;
        public float CenterY;
        public float angle;
        public float scaleX;
        public float scaleY;
        public float score;    //得分越高，模拟度越高
        public float MCHPatterWidth;
        public float MCHPatterHeight;
    }

    public class AS400LotInfo
    {
        public AS400LotInfo() { }
        public string Pinmu;     //品目號
        public string Lot_Head;
        public string Lot_End;
        public string StorageStatus;   //"3"代表已經入庫
        public string BonusLotStatus;
        public int TotalCount;
    }
}
