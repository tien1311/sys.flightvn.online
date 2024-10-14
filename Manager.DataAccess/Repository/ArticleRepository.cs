using Dapper;
using Manager.Model.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;



namespace Manager.DataAccess.Repository
{
    public class ArticleRepository
    {
        private string SQL_EV_ARTICLE;
     
        public ArticleRepository(IConfiguration configuration)
        {
            SQL_EV_ARTICLE = configuration.GetConnectionString("SQL_EV_ARTICLE");
        }

        public List<ArticleModel> Article()
        {

            List<ArticleModel> ListArticle = new List<ArticleModel>();
            string sql = @"select CreateDate,CreateEmployee,UrlImage,Title,Content_Article,Art.ID,Name as DanhMuc_Name from Article Art left join SideMenuChild Child on Child.ID = Art.IDMenuChild";
            using (var conn = new SqlConnection(SQL_EV_ARTICLE))
            {
                ListArticle = (List<ArticleModel>)conn.Query<ArticleModel>(sql, null, commandType: CommandType.Text, commandTimeout: 30);
                conn.Dispose();
            }
            //DataTable tb = db.ExecuteDataSet(sql, CommandType.Text, "server37_Article", null).Tables[0];
            //if (tb != null && tb.Rows.Count > 0)
            //{
            //    for (int i = 0; i < tb.Rows.Count; i++)
            //    {
            //        ArticleModel result = new ArticleModel();
            //        result.CreateDate = tb.Rows[i]["CreateDate"].ToString();
            //        result.CreateEmployee = tb.Rows[i]["CreateEmployee"].ToString();
            //        result.UrlImage = tb.Rows[i]["UrlImage"].ToString();
            //        result.Title = tb.Rows[i]["Title"].ToString();
            //        result.Content_Article = tb.Rows[i]["Content_Article"].ToString();
            //        result.DanhMuc_Name = tb.Rows[i]["Name"].ToString();
            //        result.ID = int.Parse(tb.Rows[i]["ID"].ToString());
            //        ListArticle.Add(result);
            //    }
            //}
            return ListArticle;
        }
        public List<SideMenu_ChildModel> SideMenuChild()
        {
            List<SideMenu_ChildModel> ListArticle = new List<SideMenu_ChildModel>();
            string sql = @"select * from SideMenuChild";
            //DataTable tb = db.ExecuteDataSet(sql, CommandType.Text, "server37_Article", null).Tables[0];
            //if (tb != null && tb.Rows.Count > 0)
            //{
            //    for (int i = 0; i < tb.Rows.Count; i++)
            //    {
            //        SideMenu_ChildModel result = new SideMenu_ChildModel();

            //        result.ID = int.Parse(tb.Rows[i]["ID"].ToString());
            //        result.Name = tb.Rows[i]["Name"].ToString();
            //        ListArticle.Add(result);
            //    }
            //}
            using (var conn = new SqlConnection(SQL_EV_ARTICLE))
            {
                ListArticle = (List<SideMenu_ChildModel>)conn.Query<SideMenu_ChildModel>(sql, null, commandType: CommandType.Text, commandTimeout: 30);
                conn.Dispose();
            }
            return ListArticle;
        }
        public List<SideMenu_ParentModel> SideMenuParent()
        {
            List<SideMenu_ParentModel> ListArticle = new List<SideMenu_ParentModel>();
            string sql = @"select * from SideMenuParent";
            //DataTable tb = db.ExecuteDataSet(sql, CommandType.Text, "server37_Article", null).Tables[0];
            //if (tb != null && tb.Rows.Count > 0)
            //{
            //    for (int i = 0; i < tb.Rows.Count; i++)
            //    {
            //        SideMenu_ParentModel result = new SideMenu_ParentModel();

            //        result.ID = int.Parse(tb.Rows[i]["ID"].ToString());
            //        result.Name = tb.Rows[i]["Name"].ToString();
            //        ListArticle.Add(result);
            //    }
            //}
            using (var conn = new SqlConnection(SQL_EV_ARTICLE))
            {
                ListArticle = (List<SideMenu_ParentModel>)conn.Query<SideMenu_ParentModel>(sql, null, commandType: CommandType.Text, commandTimeout: 30);
                conn.Dispose();
            }
            return ListArticle;
        }
        public ArticleModel EditArticle(int ID)
        {
            ArticleModel result = new ArticleModel();
            string sql = @"select Art.ID, Art.Content_Article, Art.Title,Child.ID as DanhMuc_ID,Child.Name as DanhMuc_Name from Article Art 
                            left join SideMenuChild Child on Child.ID = Art.IDMenuChild 
                            where Art.ID = '" + ID + "'";
            //DataTable tb = db.ExecuteDataSet(sql, CommandType.Text, "server37_Article", null).Tables[0];
            //if (tb != null && tb.Rows.Count > 0)
            //{

            //    result.Title = tb.Rows[0]["Title"].ToString();
            //    result.Content_Article = tb.Rows[0]["Content_Article"].ToString();
            //    result.DanhMuc_Name = tb.Rows[0]["DanhMuc_Name"].ToString();
            //    result.DanhMuc_ID = tb.Rows[0]["DanhMuc_ID"].ToString();
            //    result.ID = int.Parse(tb.Rows[0]["ID"].ToString());
            //}
            using (var conn = new SqlConnection(SQL_EV_ARTICLE))
            {
                result = conn.QueryFirst<ArticleModel>(sql, null, commandType: CommandType.Text, commandTimeout: 30);
                conn.Dispose();
            }

            List<SideMenu_ChildModel> ListArticle = new List<SideMenu_ChildModel>();
            string sqlList = @"select * from SideMenuChild";
            //DataTable tbList = db.ExecuteDataSet(sqlList, CommandType.Text, "server37_Article", null).Tables[0];
            //if (tbList != null && tbList.Rows.Count > 0)
            //{
            //    for (int i = 0; i < tbList.Rows.Count; i++)
            //    {
            //        SideMenu_ChildModel resultList = new SideMenu_ChildModel();

            //        resultList.ID = int.Parse(tbList.Rows[i]["ID"].ToString());
            //        resultList.Name = tbList.Rows[i]["Name"].ToString();
            //        ListArticle.Add(resultList);
            //    }
            //}
            using (var conn = new SqlConnection(SQL_EV_ARTICLE))
            {
                ListArticle = (List<SideMenu_ChildModel>)conn.Query<SideMenu_ChildModel>(sqlList, null, commandType: CommandType.Text, commandTimeout: 30);
                conn.Dispose();
            }
            result.ListSideMenu_Child = ListArticle;
            return result;
        }
        public bool SaveCreate(string Danhmuc, string Title, string Content, string MaNV)
        {
            int i = 0;
            string sql = "INSERT INTO [Article] ([Title] ,[Content_Article],[CreateDate],[CreateEmployee],[IDMenuChild]) VALUES ( N'" + Title + "',N'" + Content + "',GETDATE(),N'" + MaNV + "'," + Danhmuc + ")";
            //List<TangDuLieu.DBase.AddParameters> Param = new List<TangDuLieu.DBase.AddParameters>();
            //Param.Add(new DBase.AddParameters("@Title", Title));
            //Param.Add(new DBase.AddParameters("@Content_Article", Content));
            //Param.Add(new DBase.AddParameters("@CreateEmployee", MaNV));
            //Param.Add(new DBase.AddParameters("@IDMenuChild", Danhmuc));
            using (var conn = new SqlConnection(SQL_EV_ARTICLE))
            {
                i = conn.Execute(sql, null, null, commandTimeout: 30, commandType: CommandType.Text);
                conn.Dispose();
            }
            if (i > 0)
            {
                return true;
            }
            else
                return false;
        }
        public bool SaveEdit(string ID, string Danhmuc, string Title, string Content, string MaNV)
        {
            int i = 0;
            string sql = "UPDATE Article SET Title = N'" + Title + "', Content_Article = N'" + Content + "',  CreateDate = GETDATE(), CreateEmployee = N'" + MaNV + "', IDMenuChild= " + Danhmuc + " WHERE ID = " + ID;
            using (var conn = new SqlConnection(SQL_EV_ARTICLE))
            {
                i = conn.Execute(sql, null, null, commandTimeout: 30, commandType: CommandType.Text);
                conn.Dispose();
            }
            if (i > 0)
            {
                return true;
            }
            else
                return false;
        }
        public SideMenuModel SideMenu()
        {
            SideMenuModel SideMenu = new SideMenuModel();
            List<SideMenu_ParentModel> ListMenuParent = new List<SideMenu_ParentModel>();
            List<SideMenu_ChildModel> ListMenuChild = new List<SideMenu_ChildModel>();
            List<Airport> ListAirport = new List<Airport>();
            string sqlParent = @"select * from SideMenuParent";
            using (var conn = new SqlConnection(SQL_EV_ARTICLE))
            {
                ListMenuParent = (List<SideMenu_ParentModel>)conn.Query<SideMenu_ParentModel>(sqlParent, null, commandType: CommandType.Text, commandTimeout: 30);
                conn.Dispose();
            }

            string sqlChild = @"select Child.ID, Child.Name, Parent.Name as Name_Parent, Child.Status from SideMenuChild Child left join SideMenuParent Parent on Child.IDParent = Parent.ID";
            using (var conn = new SqlConnection(SQL_EV_ARTICLE))
            {
                ListMenuChild = (List<SideMenu_ChildModel>)conn.Query<SideMenu_ChildModel>(sqlChild, null, commandType: CommandType.Text, commandTimeout: 30);
                conn.Dispose();
            }
            string sqlAirport = @"select * from ListAirport";
            using (var conn = new SqlConnection(SQL_EV_ARTICLE))
            {
                ListAirport = (List<Airport>)conn.Query<Airport>(sqlAirport, null, commandType: CommandType.Text, commandTimeout: 30);
                conn.Dispose();
            }
            SideMenu.ListSideMenu_Child = ListMenuChild;
            SideMenu.ListSideMenu_Parent = ListMenuParent;
            SideMenu.ListAirport = ListAirport;
            return SideMenu;
        }
        public SideMenu_ParentModel EditMenuParent(int ID)
        {
            SideMenu_ParentModel result = new SideMenu_ParentModel();
            string sql = @"select * from SideMenuParent where ID = '" + ID + "'";
            using (var conn = new SqlConnection(SQL_EV_ARTICLE))
            {
                result = conn.QueryFirst<SideMenu_ParentModel>(sql, null, commandType: CommandType.Text, commandTimeout: 30);
                conn.Dispose();
            }
            return result;
        }
        public SideMenu_ChildModel EditMenuChild(int ID)
        {
            SideMenu_ChildModel result = new SideMenu_ChildModel();
            List<SideMenu_ParentModel> ListMenuParent = new List<SideMenu_ParentModel>();
            string sql = @"select Child.ID, Child.Name, Parent.Name as Name_Parent, Parent.ID as IDParent from SideMenuChild Child left join SideMenuParent Parent on Child.IDParent = Parent.ID where Child.ID = '" + ID + "'";
            using (var conn = new SqlConnection(SQL_EV_ARTICLE))
            {
                result = conn.QueryFirst<SideMenu_ChildModel>(sql, null, commandType: CommandType.Text, commandTimeout: 30);
                conn.Dispose();
            }
            string sqlParent = @"select * from SideMenuParent";
            using (var conn = new SqlConnection(SQL_EV_ARTICLE))
            {
                ListMenuParent = (List<SideMenu_ParentModel>)conn.Query<SideMenu_ParentModel>(sqlParent, null, commandType: CommandType.Text, commandTimeout: 30);
                conn.Dispose();
            }
            result.ListSideMenu_Parent = ListMenuParent;
            return result;
        }
        public Airport EditAirport(int ID)
        {
            Airport result = new Airport();
            string sql = @"select * from ListAirport where ID = '" + ID + "'";
            using (var conn = new SqlConnection(SQL_EV_ARTICLE))
            {
                result = conn.QueryFirst<Airport>(sql, null, commandType: CommandType.Text, commandTimeout: 30);
                conn.Dispose();
            }
            return result;
        }
        public bool StatusMenuParent(int ID, string status)
        {
            int i = 0;
            string sql = @"update SideMenuParent set Status = '" + status + "' where ID = '" + ID + "'";
            using (var conn = new SqlConnection(SQL_EV_ARTICLE))
            {
                i = conn.Execute(sql, null, null, commandTimeout: 30, commandType: CommandType.Text);
                conn.Dispose();
            }
            if (i > 0)
            {
                return true;
            }
            else
                return false;
        }
        public bool StatusMenuChild(int ID, string status)
        {
            int i = 0;
            string sql = @"update SideMenuChild set Status = '" + status + "' where ID = '" + ID + "'";
            using (var conn = new SqlConnection(SQL_EV_ARTICLE))
            {
                i = conn.Execute(sql, null, null, commandTimeout: 30, commandType: CommandType.Text);
                conn.Dispose();
            }
            if (i > 0)
            {
                return true;
            }
            else
                return false;
        }
        public bool StatusAirport(int ID, string status)
        {
            int i = 0;
            string sql = @"update ListAirport set Status = '" + status + "' where ID = '" + ID + "'";
            using (var conn = new SqlConnection(SQL_EV_ARTICLE))
            {
                i = conn.Execute(sql, null, null, commandTimeout: 30, commandType: CommandType.Text);
                conn.Dispose();
            }
            if (i > 0)
            {
                return true;
            }
            else
                return false;
        }
        public bool SaveCreateMenuParent(string Name)
        {
            int i = 0;
            string sql = "INSERT INTO [SideMenuParent] ([Name],[Status]) VALUES ( N'" + Name + "', 1)";

            using (var conn = new SqlConnection(SQL_EV_ARTICLE))
            {
                i = conn.Execute(sql, null, null, commandTimeout: 30, commandType: CommandType.Text);
                conn.Dispose();
            }
            //int i = db.ExecuteNoneQuery(sql, CommandType.Text, "server37_Article", Param);
            if (i > 0)
            {
                return true;
            }
            else
                return false;
        }
        public bool SaveCreateMenuChild(string Danhmuc, string Name)
        {
            int i = 0;
            string sql = "INSERT INTO [SideMenuChild] ([Name],[IDParent],[Status]) VALUES ( N'" + Name + "', " + Danhmuc + ", 1)";
            //List<TangDuLieu.DBase.AddParameters> Param = new List<TangDuLieu.DBase.AddParameters>();
            //Param.Add(new DBase.AddParameters("@Name", Name));
            //Param.Add(new DBase.AddParameters("@DanhMuc", Danhmuc));
            using (var conn = new SqlConnection(SQL_EV_ARTICLE))
            {
                i = conn.Execute(sql, null, null, commandTimeout: 30, commandType: CommandType.Text);
                conn.Dispose();
            }
            if (i > 0)
            {
                return true;
            }
            else
                return false;
        }
        public bool SaveCreateAirport(string Name)
        {
            int i = 0;
            string sql = "INSERT INTO [ListAirport] ([Name],Status) VALUES ( N'" + Name + "', 1)";

            using (var conn = new SqlConnection(SQL_EV_ARTICLE))
            {
                i = conn.Execute(sql, null, null, commandTimeout: 30, commandType: CommandType.Text);
                conn.Dispose();
            }
            if (i > 0)
            {
                return true;
            }
            else
                return false;
        }
        public bool SaveEditMenuParent(int ID, string Name)
        {
            int i = 0;
            string sql = "UPDATE SideMenuParent SET Name = N'" + Name + "' WHERE ID =  " + ID;
            //List<TangDuLieu.DBase.AddParameters> Param = new List<TangDuLieu.DBase.AddParameters>();
            //Param.Add(new DBase.AddParameters("@Name", Name));
            //Param.Add(new DBase.AddParameters("@ID", ID));
            using (var conn = new SqlConnection(SQL_EV_ARTICLE))
            {
                i = conn.Execute(sql, null, null, commandTimeout: 30, commandType: CommandType.Text);
                conn.Dispose();
            }
            if (i > 0)
            {
                return true;
            }
            else
                return false;
        }
        public bool SaveEditMenuChild(int ID, string Name, string Danhmuc)
        {
            int i = 0;
            string sql = "UPDATE SideMenuChild SET Name = N'" + Name + "', IDParent = " + Danhmuc + " WHERE ID = " + ID;
            //List<TangDuLieu.DBase.AddParameters> Param = new List<TangDuLieu.DBase.AddParameters>();
            //Param.Add(new DBase.AddParameters("@Name", Name));
            //Param.Add(new DBase.AddParameters("@ID", ID));
            //Param.Add(new DBase.AddParameters("@IDParent", Danhmuc));
            using (var conn = new SqlConnection(SQL_EV_ARTICLE))
            {
                i = conn.Execute(sql, null, null, commandTimeout: 30, commandType: CommandType.Text);
                conn.Dispose();
            }
            if (i > 0)
            {
                return true;
            }
            else
                return false;
        }
        public bool SaveEditAirport(int ID, string Name)
        {
            int i = 0;
            string sql = "UPDATE ListAirport SET Name = N'" + Name + "' WHERE ID =  " + ID;
            using (var conn = new SqlConnection(SQL_EV_ARTICLE))
            {
                i = conn.Execute(sql, null, null, commandTimeout: 30, commandType: CommandType.Text);
                conn.Dispose();
            }
            if (i > 0)
            {
                return true;
            }
            else
                return false;
        }
        public List<BusModel> Bus()
        {
            List<BusModel> result = new List<BusModel>();
            string sql = @"select Bus.ID,Bus.TieuDe,Bus.NoiDung,Bus.IDAirport, ListAirport.Name as AirportName from bus left join ListAirport on Bus.IDAirport = ListAirport.ID";
            using (var conn = new SqlConnection(SQL_EV_ARTICLE))
            {
                result = (List<BusModel>)conn.Query<BusModel>(sql, null, commandType: CommandType.Text, commandTimeout: 30);
                conn.Dispose();
            }
            return result;
        }
        public List<Airport> ListAirport()
        {
            List<Airport> ListAirport = new List<Airport>();
            string sqlAirport = @"select * from ListAirport where Status = 1";
            using (var conn = new SqlConnection(SQL_EV_ARTICLE))
            {
                ListAirport = (List<Airport>)conn.Query<Airport>(sqlAirport, null, commandType: CommandType.Text, commandTimeout: 30);
                conn.Dispose();
            }
            return ListAirport;
        }
        public bool SaveCreateBus(string Danhmuc, string Title, string Content)
        {
            int i = 0;
            string sql = "INSERT INTO [Bus] ([TieuDe] ,[NoiDung],[IDAirport],[Status]) VALUES ( N'" + Title + "',N'" + Content + "'," + Danhmuc + ",1)";
            using (var conn = new SqlConnection(SQL_EV_ARTICLE))
            {
                i = conn.Execute(sql, null, null, commandTimeout: 30, commandType: CommandType.Text);
                conn.Dispose();
            }
            if (i > 0)
            {
                return true;
            }
            else
                return false;
        }
        public BusModel EditBus(int ID)
        {
            BusModel result = new BusModel();
            string sql = @"select Bus.ID,Bus.TieuDe,Bus.NoiDung,Bus.IDAirport, ListAirport.Name as AirportName from bus left join ListAirport on Bus.IDAirport = ListAirport.ID where Bus.ID = '" + ID + "'";

            using (var conn = new SqlConnection(SQL_EV_ARTICLE))
            {
                result = conn.QueryFirst<BusModel>(sql, null, commandType: CommandType.Text, commandTimeout: 30);
                conn.Dispose();
            }
            List<Airport> ListAir = ListAirport();
            result.ListAirport = ListAir;
            return result;
        }
        public MapModel Map()
        {
            MapModel result = new MapModel();
            List<Map_QN> QN = new List<Map_QN>();
            List<Map_QT> QT = new List<Map_QT>();

            string sqlQN = @"select Map.ID,Map.TieuDe,Map.NoiDung,Map.IDAirport, Map.Loai, ListAirport.Name as AirportName from Map left join ListAirport on Map.IDAirport = ListAirport.ID where Loai = 'QN'";
            using (var conn = new SqlConnection(SQL_EV_ARTICLE))
            {
                QN = (List<Map_QN>)conn.Query<Map_QN>(sqlQN, null, commandType: CommandType.Text, commandTimeout: 30);
                conn.Dispose();
            }
            string sqlQT = @"select Map.ID,Map.TieuDe,Map.NoiDung,Map.IDAirport, Map.Loai, ListAirport.Name as AirportName from Map left join ListAirport on Map.IDAirport = ListAirport.ID where Loai = 'QT'";
            using (var conn = new SqlConnection(SQL_EV_ARTICLE))
            {
                QT = (List<Map_QT>)conn.Query<Map_QT>(sqlQT, null, commandType: CommandType.Text, commandTimeout: 30);
                conn.Dispose();
            }
            result.Map_QN = QN;
            result.Map_QT = QT;
            return result;
        }
        public bool SaveCreateMap(string Danhmuc, string Title, string Content, string Loai)
        {
            int i = 0;
            string sql = "INSERT INTO [Map] ([TieuDe] ,[NoiDung],[IDAirport],[Loai],[Status]) VALUES ( N'" + Title + "',N'" + Content + "'," + Danhmuc + ",'" + Loai + "',1)";
            using (var conn = new SqlConnection(SQL_EV_ARTICLE))
            {
                i = conn.Execute(sql, null, null, commandTimeout: 30, commandType: CommandType.Text);
                conn.Dispose();
            }
            if (i > 0)
            {
                return true;
            }
            else
                return false;
        }
        public Map_QN EditMap(int ID)
        {
            Map_QN result = new Map_QN();
            string sql = @"select Map.ID,Map.TieuDe,Map.NoiDung,Map.IDAirport,Map.Loai, ListAirport.Name as AirportName from Map left join ListAirport on Map.IDAirport = ListAirport.ID where Map.ID = '" + ID + "'";

            using (var conn = new SqlConnection(SQL_EV_ARTICLE))
            {
                result = conn.QueryFirst<Map_QN>(sql, null, commandType: CommandType.Text, commandTimeout: 30);
                conn.Dispose();
            }
            List<Airport> ListAir = ListAirport();
            result.ListAirport = ListAir;
            return result;
        }
        public bool SaveEditBus(string ID, string Danhmuc, string Title, string Content)
        {
            int i = 0;
            string sql = "UPDATE Bus SET TieuDe = N'" + Title + "', NoiDung = N'" + Content + "', IDAirport= " + Danhmuc + " WHERE ID = " + ID;
            using (var conn = new SqlConnection(SQL_EV_ARTICLE))
            {
                i = conn.Execute(sql, null, null, commandTimeout: 30, commandType: CommandType.Text);
                conn.Dispose();
            }
            if (i > 0)
            {
                return true;
            }
            else
                return false;
        }
        public bool SaveEditMap(string ID, string Danhmuc, string Title, string Content, string Loai)
        {
            int i = 0;
            string sql = "UPDATE Map SET TieuDe = N'" + Title + "', NoiDung = N'" + Content + "', IDAirport= " + Danhmuc + ", Loai = N'" + Loai + "' WHERE ID = '" + ID + "'";
            using (var conn = new SqlConnection(SQL_EV_ARTICLE))
            {
                i = conn.Execute(sql, null, null, commandTimeout: 30, commandType: CommandType.Text);
                conn.Dispose();
            }
            if (i > 0)
            {
                return true;
            }
            else
                return false;
        }
    }
}
