using Manager.Model.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using TangDuLieu;

namespace Manager.DataAccess.Repository
{
    public class SanPhamDichVuRepository
    {
        DBase db = new DBase();

        public List<SanPhamDichVuModel> SanPham()
        {
            List<SanPhamDichVuModel> List = new List<SanPhamDichVuModel>();
            string sql = @"select * from PRODUCTPARENT where STATUS = 1";
            DataTable tb = db.ExecuteDataSet(sql, CommandType.Text, "server18", null).Tables[0];
            if (tb != null && tb.Rows.Count > 0)
            {
                for (int i = 0; i < tb.Rows.Count; i++)
                {
                    SanPhamDichVuModel result = new SanPhamDichVuModel();
                    result.ID = tb.Rows[i]["ID"].ToString();
                    result.Name = tb.Rows[i]["NAME"].ToString();
                    result.MainImg = tb.Rows[i]["MAINIMG"].ToString();
                    result.ID_PRODUCT = tb.Rows[i]["ID_PRODUCT"].ToString();
                    List.Add(result);
                }
            }
            return List;
        }
        public SanPhamDichVuModel EditSanPham(int ID)
        {
            SanPhamDichVuModel result = new SanPhamDichVuModel();
            string sql = @"select * from PRODUCTPARENT where ID = '" + ID + "'";
            DataTable tb = db.ExecuteDataSet(sql, CommandType.Text, "server18", null).Tables[0];
            if (tb != null && tb.Rows.Count > 0)
            {
                for (int i = 0; i < tb.Rows.Count; i++)
                {
                    result.ID = tb.Rows[i]["ID"].ToString();
                    result.Name = tb.Rows[i]["NAME"].ToString();
                    result.Price = tb.Rows[i]["PRICESHOW"].ToString();
                    result.PriceLogin = tb.Rows[i]["PRICEAGENT"].ToString();
                    result.Content = tb.Rows[i]["CONTENTDATA"].ToString();
                    result.Description = tb.Rows[i]["DESCRIPTION"].ToString();
                    result.MainImg = tb.Rows[i]["MAINIMG"].ToString();
                }
            }
            return result;
        }
        public bool SaveCreateSanPham(string Name, string GiaShow, string GiaDN, string MoTa, string CreateContent, string MaNV, string file)
        {
            var ID_Product = "SP" + DateTime.Now.ToString("yyMMddHHmmss");
            string sql = "INSERT INTO [PRODUCTPARENT] ([NAME] ,[DESCRIPTION],[CONTENTDATA],[CREATEEMPL],[CREATEDATE],[PRICESHOW],[PRICEAGENT],[STATUS],[MAINIMG],[ID_PRODUCT]) VALUES ( @NAME,@DESCRIPTION,@CONTENTDATA,@CREATEEMPL,GETDATE(),@PRICESHOW,@PRICEAGENT,1,@FILE,@ID_PRODUCT)";
            List<DBase.AddParameters> Param = new List<DBase.AddParameters>();
            Param.Add(new DBase.AddParameters("@NAME", Name));
            Param.Add(new DBase.AddParameters("@DESCRIPTION", MoTa));
            Param.Add(new DBase.AddParameters("@CONTENTDATA", CreateContent));
            Param.Add(new DBase.AddParameters("@CREATEEMPL", MaNV));
            Param.Add(new DBase.AddParameters("@PRICESHOW", GiaShow));
            Param.Add(new DBase.AddParameters("@PRICEAGENT", GiaDN));
            Param.Add(new DBase.AddParameters("@FILE", file));
            Param.Add(new DBase.AddParameters("@ID_PRODUCT", ID_Product));
            int i = db.ExecuteNoneQuery(sql, CommandType.Text, "server18", Param);
            if (i > 0)
            {
                return true;
            }
            else
                return false;
        }
        public bool SaveEditSanPham(int ID, string Name, string GiaShow, string GiaDN, string MoTa, string CreateContent, string MaNV, string file)
        {
            string sql = "UPDATE PRODUCTPARENT SET NAME = @NAME, DESCRIPTION = @DESCRIPTION,  CONTENTDATA = @CONTENTDATA, CREATEDATE = GETDATE(),CREATEEMPL = @CREATEEMPL, PRICESHOW = @PRICESHOW, PRICEAGENT = @PRICEAGENT, MAINIMG = @MAINIMG WHERE ID = @ID ";
            List<DBase.AddParameters> Param = new List<DBase.AddParameters>();
            Param.Add(new DBase.AddParameters("@ID", ID));
            Param.Add(new DBase.AddParameters("@NAME", Name));
            Param.Add(new DBase.AddParameters("@DESCRIPTION", MoTa));
            Param.Add(new DBase.AddParameters("@CONTENTDATA", CreateContent));
            Param.Add(new DBase.AddParameters("@CREATEEMPL", MaNV));
            Param.Add(new DBase.AddParameters("@PRICESHOW", GiaShow));
            Param.Add(new DBase.AddParameters("@PRICEAGENT", GiaDN));
            Param.Add(new DBase.AddParameters("@MAINIMG", file));

            int i = db.ExecuteNoneQuery(sql, CommandType.Text, "server18", Param);
            if (i > 0)
            {
                return true;
            }
            else
                return false;
        }
        public List<SanPhamDichVuModel> DichVu()
        {
            List<SanPhamDichVuModel> List = new List<SanPhamDichVuModel>();
            string sql = @"select * from PRODUCTPARENT where STATUS = 0";
            DataTable tb = db.ExecuteDataSet(sql, CommandType.Text, "server18", null).Tables[0];
            if (tb != null && tb.Rows.Count > 0)
            {
                for (int i = 0; i < tb.Rows.Count; i++)
                {
                    SanPhamDichVuModel result = new SanPhamDichVuModel();
                    result.ID = tb.Rows[i]["ID"].ToString();
                    result.Name = tb.Rows[i]["NAME"].ToString();
                    result.MainImg = tb.Rows[i]["MAINIMG"].ToString();
                    result.ID_PRODUCT = tb.Rows[i]["ID_PRODUCT"].ToString();
                    List.Add(result);
                }
            }
            return List;
        }
        public SanPhamDichVuModel EditDichVu(int ID)
        {
            SanPhamDichVuModel result = new SanPhamDichVuModel();
            string sql = @"select * from PRODUCTPARENT where ID = '" + ID + "'";
            DataTable tb = db.ExecuteDataSet(sql, CommandType.Text, "server18", null).Tables[0];
            if (tb != null && tb.Rows.Count > 0)
            {
                for (int i = 0; i < tb.Rows.Count; i++)
                {
                    result.ID = tb.Rows[i]["ID"].ToString();
                    result.Name = tb.Rows[i]["NAME"].ToString();
                    result.Price = tb.Rows[i]["PRICESHOW"].ToString();
                    result.PriceLogin = tb.Rows[i]["PRICEAGENT"].ToString();
                    result.Content = tb.Rows[i]["CONTENTDATA"].ToString();
                    result.Description = tb.Rows[i]["DESCRIPTION"].ToString();
                    result.MainImg = tb.Rows[i]["MAINIMG"].ToString();
                }
            }
            return result;
        }
        public bool SaveCreateDichVu(string Name, string GiaShow, string GiaDN, string MoTa, string CreateContent, string MaNV, string file)
        {
            var ID_Product = "DV" + DateTime.Now.ToString("yyMMddHHmmss");
            string sql = "INSERT INTO [PRODUCTPARENT] ([NAME] ,[DESCRIPTION],[CONTENTDATA],[CREATEEMPL],[CREATEDATE],[PRICESHOW],[PRICEAGENT],[STATUS],[MAINIMG],[ID_PRODUCT]) VALUES ( @NAME,@DESCRIPTION,@CONTENTDATA,@CREATEEMPL,GETDATE(),@PRICESHOW,@PRICEAGENT,0,@FILE,@ID_PRODUCT)";
            List<DBase.AddParameters> Param = new List<DBase.AddParameters>();
            Param.Add(new DBase.AddParameters("@NAME", Name));
            Param.Add(new DBase.AddParameters("@DESCRIPTION", MoTa));
            Param.Add(new DBase.AddParameters("@CONTENTDATA", CreateContent));
            Param.Add(new DBase.AddParameters("@CREATEEMPL", MaNV));
            Param.Add(new DBase.AddParameters("@PRICESHOW", GiaShow));
            Param.Add(new DBase.AddParameters("@PRICEAGENT", GiaDN));
            Param.Add(new DBase.AddParameters("@FILE", file));
            Param.Add(new DBase.AddParameters("@ID_PRODUCT", ID_Product));

            int i = db.ExecuteNoneQuery(sql, CommandType.Text, "server18", Param);
            if (i > 0)
            {
                return true;
            }
            else
                return false;
        }
        public bool SaveEditDichVu(int ID, string Name, string GiaShow, string GiaDN, string MoTa, string CreateContent, string MaNV, string file)
        {
            string sql = "UPDATE PRODUCTPARENT SET NAME = @NAME, DESCRIPTION = @DESCRIPTION,  CONTENTDATA = @CONTENTDATA, CREATEDATE = GETDATE(),CREATEEMPL = @CREATEEMPL, PRICESHOW = @PRICESHOW, PRICEAGENT = @PRICEAGENT, MAINIMG = @MAINIMG WHERE ID = @ID ";
            List<DBase.AddParameters> Param = new List<DBase.AddParameters>();
            Param.Add(new DBase.AddParameters("@ID", ID));
            Param.Add(new DBase.AddParameters("@NAME", Name));
            Param.Add(new DBase.AddParameters("@DESCRIPTION", MoTa));
            Param.Add(new DBase.AddParameters("@CONTENTDATA", CreateContent));
            Param.Add(new DBase.AddParameters("@CREATEEMPL", MaNV));
            Param.Add(new DBase.AddParameters("@PRICESHOW", GiaShow));
            Param.Add(new DBase.AddParameters("@PRICEAGENT", GiaDN));
            Param.Add(new DBase.AddParameters("@MAINIMG", file));

            int i = db.ExecuteNoneQuery(sql, CommandType.Text, "server18", Param);
            if (i > 0)
            {
                return true;
            }
            else
                return false;
        }
        public List<SanPhamChild> SanPhamChild()
        {
            List<SanPhamChild> List = new List<SanPhamChild>();
            string sql = @"select * from PRODUCTCHILD where STATUS = 1";
            DataTable tb = db.ExecuteDataSet(sql, CommandType.Text, "server18", null).Tables[0];
            if (tb != null && tb.Rows.Count > 0)
            {
                for (int i = 0; i < tb.Rows.Count; i++)
                {
                    SanPhamChild result = new SanPhamChild();
                    result.ID = tb.Rows[i]["ID"].ToString();
                    result.Name = tb.Rows[i]["NAME"].ToString();
                    result.ID_PRODUCTCHILD = tb.Rows[i]["ID_PRODUCTCHILD"].ToString();
                    result.ChildImg = tb.Rows[i]["CHILDIMG"].ToString();
                    string sqlParent = @"select * from PRODUCTPARENT where ID_PRODUCT = '" + tb.Rows[i]["IDParent"].ToString() + "'";
                    DataTable tbParent = db.ExecuteDataSet(sqlParent, CommandType.Text, "server18", null).Tables[0];
                    if (tbParent != null && tbParent.Rows.Count > 0)
                    {
                        result.NameParent = tbParent.Rows[0]["NAME"].ToString();
                    }
                    List.Add(result);
                }
            }
            return List;
        }
        public SanPhamChild EditSanPhamChild(int ID)
        {
            SanPhamChild result = new SanPhamChild();
            string sql = @"select * from PRODUCTCHILD where ID = '" + ID + "'";
            DataTable tb = db.ExecuteDataSet(sql, CommandType.Text, "server18", null).Tables[0];
            if (tb != null && tb.Rows.Count > 0)
            {
                for (int i = 0; i < tb.Rows.Count; i++)
                {
                    result.ID = tb.Rows[i]["ID"].ToString();
                    result.Name = tb.Rows[i]["NAME"].ToString();
                    result.Price = tb.Rows[i]["PRICESHOW"].ToString();
                    result.PriceLogin = tb.Rows[i]["PRICEAGENT"].ToString();
                    result.ChildImg = tb.Rows[i]["CHILDIMG"].ToString();
                    result.IDParent = tb.Rows[i]["IDPARENT"].ToString();
                }
            }
            return result;
        }
        public bool SaveCreateSanPhamChild(string Name, string GiaShow, string GiaDN, string MaNV, string file, string ID_Parent)
        {
            var ID_Product = ID_Parent + "-" + PhatSinhMaSPDVChild(ID_Parent);
            string sql = "INSERT INTO [PRODUCTCHILD] ([NAME],[CREATEEMPL],[CREATEDATE],[PRICESHOW],[PRICEAGENT],[STATUS],[CHILDIMG],[ID_PRODUCTCHILD],[IDPARENT]) VALUES ( @NAME,@CREATEEMPL,GETDATE(),@PRICESHOW,@PRICEAGENT,1,@FILE,@ID_PRODUCTCHILD,@IDPARENT)";
            List<DBase.AddParameters> Param = new List<DBase.AddParameters>();
            Param.Add(new DBase.AddParameters("@NAME", Name));
            Param.Add(new DBase.AddParameters("@CREATEEMPL", MaNV));
            Param.Add(new DBase.AddParameters("@PRICESHOW", GiaShow));
            Param.Add(new DBase.AddParameters("@PRICEAGENT", GiaDN));
            Param.Add(new DBase.AddParameters("@FILE", file));
            Param.Add(new DBase.AddParameters("@ID_PRODUCTCHILD", ID_Product));
            Param.Add(new DBase.AddParameters("@IDPARENT", ID_Parent));
            int i = db.ExecuteNoneQuery(sql, CommandType.Text, "server18", Param);
            if (i > 0)
            {
                return true;
            }
            else
                return false;
        }
        public bool SaveEditSanPhamChild(int ID, string Name, string GiaShow, string GiaDN, string MaNV, string file, string ID_Parent)
        {
            string sql = "UPDATE PRODUCTCHILD SET NAME = @NAME, CREATEDATE = GETDATE(),CREATEEMPL = @CREATEEMPL, PRICESHOW = @PRICESHOW, PRICEAGENT = @PRICEAGENT, CHILDIMG = @CHILDIMG WHERE ID = @ID ";
            List<DBase.AddParameters> Param = new List<DBase.AddParameters>();
            Param.Add(new DBase.AddParameters("@NAME", Name));
            Param.Add(new DBase.AddParameters("@CREATEEMPL", MaNV));
            Param.Add(new DBase.AddParameters("@PRICESHOW", GiaShow));
            Param.Add(new DBase.AddParameters("@PRICEAGENT", GiaDN));
            Param.Add(new DBase.AddParameters("@CHILDIMG", file));
            Param.Add(new DBase.AddParameters("@IDPARENT", ID_Parent));
            Param.Add(new DBase.AddParameters("@ID", ID));

            int i = db.ExecuteNoneQuery(sql, CommandType.Text, "server18", Param);
            if (i > 0)
            {
                return true;
            }
            else
                return false;
        }
        public List<SanPhamChild> DichVuChild()
        {
            List<SanPhamChild> List = new List<SanPhamChild>();
            string sql = @"select * from PRODUCTCHILD where STATUS = 0";
            DataTable tb = db.ExecuteDataSet(sql, CommandType.Text, "server18", null).Tables[0];
            if (tb != null && tb.Rows.Count > 0)
            {
                for (int i = 0; i < tb.Rows.Count; i++)
                {
                    SanPhamChild result = new SanPhamChild();
                    result.ID = tb.Rows[i]["ID"].ToString();
                    result.Name = tb.Rows[i]["NAME"].ToString();
                    result.ID_PRODUCTCHILD = tb.Rows[i]["ID_PRODUCTCHILD"].ToString();
                    result.ChildImg = tb.Rows[i]["CHILDIMG"].ToString();
                    string sqlParent = @"select * from PRODUCTPARENT where ID_PRODUCT = '" + tb.Rows[i]["IDParent"].ToString() + "'";
                    DataTable tbParent = db.ExecuteDataSet(sqlParent, CommandType.Text, "server18", null).Tables[0];
                    if (tbParent != null && tbParent.Rows.Count > 0)
                    {
                        result.NameParent = tbParent.Rows[0]["NAME"].ToString();
                    }
                    List.Add(result);
                }
            }
            return List;
        }
        public SanPhamChild EditDichVuChild(int ID)
        {
            SanPhamChild result = new SanPhamChild();
            string sql = @"select * from PRODUCTCHILD where ID = '" + ID + "'";
            DataTable tb = db.ExecuteDataSet(sql, CommandType.Text, "server18", null).Tables[0];
            if (tb != null && tb.Rows.Count > 0)
            {
                for (int i = 0; i < tb.Rows.Count; i++)
                {
                    result.ID = tb.Rows[i]["ID"].ToString();
                    result.Name = tb.Rows[i]["NAME"].ToString();
                    result.Price = tb.Rows[i]["PRICESHOW"].ToString();
                    result.PriceLogin = tb.Rows[i]["PRICEAGENT"].ToString();
                    result.ChildImg = tb.Rows[i]["CHILDIMG"].ToString();
                    result.IDParent = tb.Rows[i]["IDPARENT"].ToString();
                }
            }
            return result;
        }
        public bool SaveCreateDichVuChild(string Name, string GiaShow, string GiaDN, string MaNV, string file, string ID_Parent)
        {
            var ID_Product = ID_Parent + "-" + PhatSinhMaSPDVChild(ID_Parent);
            string sql = "INSERT INTO [PRODUCTCHILD] ([NAME],[CREATEEMPL],[CREATEDATE],[PRICESHOW],[PRICEAGENT],[STATUS],[CHILDIMG],[ID_PRODUCTCHILD],[IDPARENT]) VALUES ( @NAME,@CREATEEMPL,GETDATE(),@PRICESHOW,@PRICEAGENT,0,@FILE,@ID_PRODUCTCHILD,@IDPARENT)";
            List<DBase.AddParameters> Param = new List<DBase.AddParameters>();
            Param.Add(new DBase.AddParameters("@NAME", Name));
            Param.Add(new DBase.AddParameters("@CREATEEMPL", MaNV));
            Param.Add(new DBase.AddParameters("@PRICESHOW", GiaShow));
            Param.Add(new DBase.AddParameters("@PRICEAGENT", GiaDN));
            Param.Add(new DBase.AddParameters("@FILE", file));
            Param.Add(new DBase.AddParameters("@ID_PRODUCTCHILD", ID_Product));
            Param.Add(new DBase.AddParameters("@IDPARENT", ID_Parent));
            int i = db.ExecuteNoneQuery(sql, CommandType.Text, "server18", Param);
            if (i > 0)
            {
                return true;
            }
            else
                return false;
        }
        public bool SaveEditDichVuChild(int ID, string Name, string GiaShow, string GiaDN, string MaNV, string file, string ID_Parent)
        {
            string sql = "UPDATE PRODUCTCHILD SET NAME = @NAME, CREATEDATE = GETDATE(),CREATEEMPL = @CREATEEMPL, PRICESHOW = @PRICESHOW, PRICEAGENT = @PRICEAGENT, CHILDIMG = @CHILDIMG WHERE ID = @ID ";
            List<DBase.AddParameters> Param = new List<DBase.AddParameters>();
            Param.Add(new DBase.AddParameters("@NAME", Name));
            Param.Add(new DBase.AddParameters("@CREATEEMPL", MaNV));
            Param.Add(new DBase.AddParameters("@PRICESHOW", GiaShow));
            Param.Add(new DBase.AddParameters("@PRICEAGENT", GiaDN));
            Param.Add(new DBase.AddParameters("@CHILDIMG", file));
            Param.Add(new DBase.AddParameters("@IDPARENT", ID_Parent));
            Param.Add(new DBase.AddParameters("@ID", ID));

            int i = db.ExecuteNoneQuery(sql, CommandType.Text, "server18", Param);
            if (i > 0)
            {
                return true;
            }
            else
                return false;
        }
        public string PhatSinhMaSPDVChild(string ID_Parent)
        {
            try
            {
                string Maphieu = "", sql = "";
                if (ID_Parent.Substring(0, 2) == "SP")
                {
                    sql = @"select top 1 ID_PRODUCTCHILD from PRODUCTCHILD where ID_PRODUCTCHILD like N'SP%' order by CREATEDATE Desc ";
                }
                if (ID_Parent.Substring(0, 2) == "DV")
                {
                    sql = @"select top 1 ID_PRODUCTCHILD from PRODUCTCHILD where ID_PRODUCTCHILD like N'DV%' order by CREATEDATE Desc ";
                }
                DataTable tb = db.ExecuteDataSet(sql, CommandType.Text, "server18", null).Tables[0];
                if (tb != null)
                {
                    if (tb.Rows.Count > 0)
                    {
                        string ten = tb.Rows[0]["ID_PRODUCTCHILD"].ToString();
                        string soThuTu = ten.Substring(15, 3);
                        int STT = int.Parse(soThuTu) + 1;
                        if (STT > 0 && STT < 10)
                        {
                            Maphieu = "00" + STT;
                        }
                        if (STT >= 10 && STT < 100)
                        {
                            Maphieu = "0" + STT;
                        }
                        if (STT >= 100 && STT < 1000)
                        {
                            Maphieu = "" + STT;
                        }
                    }
                    else
                    {
                        Maphieu = "001";
                    }
                }
                else
                {
                    Maphieu = "001";
                }
                return Maphieu;
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
