using Dapper;
using Manager.Model.Models.LandingPage;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manager.DataAccess.Repository
{
    public class LandingPageRepository
    {
        private string _connectionString;
        private IConfiguration _configuration;
        public LandingPageRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("SQL_LANDINGPAGE");
        }

        #region GET

        public async Task<Logo> GetLogo()
        {
            Logo logo = new Logo();
            string sql = @"SELECT * FROM Logo";
            using (var conn = new SqlConnection(_connectionString))
            {
                await conn.OpenAsync();
                logo = conn.QueryAsync<Logo>(sql).GetAwaiter().GetResult().FirstOrDefault();
                conn.Close();
            }
            return logo;
        }

        public async Task<List<CompanyInfo>> GetCompanyInfo()
        {
            List<CompanyInfo> companyInfo = new List<CompanyInfo>();
            string sql = @"SELECT * FROM CompanyInfo";
            using (var conn = new SqlConnection(_connectionString))
            {
                await conn.OpenAsync();
                companyInfo = conn.QueryAsync<CompanyInfo>(sql).GetAwaiter().GetResult().ToList();
                conn.Close();
            }
            return companyInfo;
        }

        public async Task<CompanyInfo> GetCompanyInfo(int id)
        {
            CompanyInfo companyInfo = new CompanyInfo();
            string sql = @"SELECT * FROM CompanyInfo WHERE Id = @id";
            using (var conn = new SqlConnection(_connectionString))
            {
                await conn.OpenAsync();
                companyInfo = conn.QueryAsync<CompanyInfo>(sql, new { id = id }).GetAwaiter().GetResult().FirstOrDefault();
                conn.Close();
            }
            return companyInfo;
        }

        public async Task<List<PartnerBanner>> GetPartnerBanner()
        {
            List<PartnerBanner> partnerBanners = new List<PartnerBanner>();
            string sql = @"SELECT * FROM PartnerBanner";
            using (var conn = new SqlConnection(_connectionString))
            {
                await conn.OpenAsync();
                partnerBanners = conn.QueryAsync<PartnerBanner>(sql).GetAwaiter().GetResult().ToList();
                conn.Close();
            }
            return partnerBanners;
        }

        public async Task<PartnerBanner> GetPartnerBanner(int Id)
        {
            PartnerBanner partnerBanner = new PartnerBanner();
            string sql = @"SELECT * FROM PartnerBanner WHERE Id = @id";
            using (var conn = new SqlConnection(_connectionString))
            {
                await conn.OpenAsync();
                partnerBanner = conn.QueryAsync<PartnerBanner>(sql, new { id = Id }).GetAwaiter().GetResult().FirstOrDefault();
                conn.Close();
            }
            return partnerBanner;
        }

        public async Task<List<Service>> GetService()
        {
            List<Service> listService = new List<Service>();
            string sql = @"SELECT * FROM Service";
            using (var conn = new SqlConnection(_connectionString))
            {
                await conn.OpenAsync();
                listService = conn.QueryAsync<Service>(sql).GetAwaiter().GetResult().ToList();
                conn.Close();
            }
            return listService;
        }

        public async Task<Service> GetService(int Id)
        {
            Service service = new Service();
            string sql = @"SELECT * FROM Service WHERE Id = @id";
            using (var conn = new SqlConnection(_connectionString))
            {
                await conn.OpenAsync();
                service = conn.QueryAsync<Service>(sql, new { id = Id }).GetAwaiter().GetResult().FirstOrDefault();
                conn.Close();
            }
            return service;
        }

        public async Task<List<SocialMedia>> GetSocialMedia()
        {
            List<SocialMedia> listSocialMedia = new List<SocialMedia>();
            string sql = @"SELECT * FROM SocialMedia";
            using (var conn = new SqlConnection(_connectionString))
            {
                await conn.OpenAsync();
                listSocialMedia = conn.QueryAsync<SocialMedia>(sql).GetAwaiter().GetResult().ToList();
                conn.Close();
            }
            return listSocialMedia;
        }

        public async Task<SocialMedia> GetSocialMedia(int Id)
        {
            SocialMedia socialMedia = new SocialMedia();
            string sql = @"SELECT * FROM SocialMedia WHERE Id = @id";
            using (var conn = new SqlConnection(_connectionString))
            {
                await conn.OpenAsync();
                socialMedia = conn.QueryAsync<SocialMedia>(sql, new { id = Id }).GetAwaiter().GetResult().FirstOrDefault();
                conn.Close();
            }
            return socialMedia;
        }

        public async Task<List<Slider>> GetSlider()
        {
            List<Slider> listSlider = new List<Slider>();
            string sql = @"SELECT * FROM Slider";
            using (var conn = new SqlConnection(_connectionString))
            {
                await conn.OpenAsync();
                listSlider = conn.QueryAsync<Slider>(sql).GetAwaiter().GetResult().ToList();
                conn.Close();
            }
            return listSlider;
        }

        public async Task<Slider> GetSlider(int Id)
        {
            Slider slider = new Slider();
            string sql = @"SELECT * FROM Slider WHERE Id = @id";
            using (var conn = new SqlConnection(_connectionString))
            {
                await conn.OpenAsync();
                slider = conn.QueryAsync<Slider>(sql, new { id = Id }).GetAwaiter().GetResult().FirstOrDefault();
                conn.Close();
            }
            return slider;
        }


        public async Task<List<Category>> GetCategory()
        {
            List<Category> listCategory = new List<Category>();
            string sql = @"SELECT * FROM Category ORDER BY Position";
            using (var conn = new SqlConnection(_connectionString))
            {
                await conn.OpenAsync();
                listCategory = conn.QueryAsync<Category>(sql).GetAwaiter().GetResult().ToList();
                conn.Close();
            }
            return listCategory;
        }

        public async Task<Category> GetCategory(int Id)
        {
            Category category = new Category();
            string sql = @"SELECT * FROM Category WHERE Id = @id";
            using (var conn = new SqlConnection(_connectionString))
            {
                await conn.OpenAsync();
                category = conn.QueryAsync<Category>(sql, new { id = Id }).GetAwaiter().GetResult().FirstOrDefault();
                conn.Close();
            }
            return category;
        }

        public async Task<List<Category>> GetCategoryForPost()
        {
            List<Category> category = new List<Category>();
            string sql = @"
                            SELECT c.*
                            FROM dbo.Category c
                            LEFT JOIN dbo.Post p ON c.Id = p.CategoryId
                            WHERE p.Id IS NULL; ";
            using (var conn = new SqlConnection(_connectionString))
            {
                await conn.OpenAsync();
                category = conn.QueryAsync<Category>(sql).GetAwaiter().GetResult().ToList();
                conn.Close();
            }
            return category;
        }

        public async Task<List<Post>> GetPost()
        {
            List<Post> listPost = new List<Post>();
            string sql = @"SELECT * FROM Post";
            using (var conn = new SqlConnection(_connectionString))
            {
                await conn.OpenAsync();
                listPost = conn.QueryAsync<Post>(sql).GetAwaiter().GetResult().ToList();
                conn.Close();
            }
            return listPost;
        }

        public async Task<Post> GetPost(int Id)
        {
            Post post = new Post();
            string sql = @"SELECT * FROM Post WHERE Id = @id";
            using (var conn = new SqlConnection(_connectionString))
            {
                await conn.OpenAsync();
                post = conn.QueryAsync<Post>(sql, new { id = Id }).GetAwaiter().GetResult().FirstOrDefault();
                conn.Close();
            }
            return post;
        }

        public async Task<List<CompanyLocation>> GetCompanyLocation()
        {
            List<CompanyLocation> list = new List<CompanyLocation>();
            string sql = @"SELECT * FROM CompanyLocation";
            using (var conn = new SqlConnection(_connectionString))
            {
                await conn.OpenAsync();
                list = conn.QueryAsync<CompanyLocation>(sql).GetAwaiter().GetResult().ToList();
                conn.Close();
            }
            return list;
        }

        public (List<News>, int) GetListNews(News request, DateTime fromDate, DateTime toDate, int pageNumber, int pageSize)
        {
            List<News> listNews = new List<News>();
            int totalRecords = 0;
            var parameters = new DynamicParameters();

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var query = new StringBuilder(@"
                        SELECT * FROM (
                            SELECT ROW_NUMBER() OVER (ORDER BY Id DESC) AS RowNum, 
                                * 
                            FROM News 
                            WHERE 1 = 1");

                var queryCount = new StringBuilder(@"
                        SELECT COUNT(*) FROM News WHERE 1=1");

                if (!string.IsNullOrEmpty(request.CreatedBy))
                {
                    query.Append(" AND CreatedBy LIKE @CreatedBy");
                    queryCount.Append(" AND CreatedBy LIKE @CreatedBy");
                }

                if (fromDate != DateTime.MinValue && toDate != DateTime.MinValue)
                {
                    query.Append(" AND CreatedDate BETWEEN @StartDate AND @EndDate");
                    queryCount.Append(" AND CreatedDate BETWEEN @StartDate AND @EndDate");
                }
                else if (fromDate != DateTime.MinValue && toDate == DateTime.MinValue)
                {
                    query.Append(" AND CreatedDate >= @StartDate");
                    queryCount.Append(" AND CreatedDate >= @StartDate");

                }
                else if (fromDate == DateTime.MinValue && toDate != DateTime.MinValue)
                {
                    query.Append(" AND CreatedDate <= @EndDate");
                    queryCount.Append(" AND CreatedDate <= @EndDate");
                }


                query.Append(@"
                            ) AS News
                            WHERE RowNum BETWEEN ((@PageNumber - 1) * @PageSize + 1) AND (@PageNumber * @PageSize)
                            ORDER BY RowNum");

                // Tạo đối tượng tham số
                parameters.Add("CreatedBy", !string.IsNullOrEmpty(request.CreatedBy) ? $"%{request.CreatedBy}%" : string.Empty);
                parameters.Add("StartDate", fromDate);
                parameters.Add("EndDate", toDate);
                parameters.Add("PageNumber", pageNumber);
                parameters.Add("PageSize", pageSize);

                listNews = connection.Query<News>(query.ToString(), parameters).ToList();
                totalRecords = connection.QuerySingle<int>(queryCount.ToString(), parameters);
                connection.Close();
            }
            return (listNews, totalRecords);
        }

        


        public async Task<News> GetNews(int Id)
        {
            News post = new News();
            string sql = @"SELECT * FROM News WHERE Id = @id";
            using (var conn = new SqlConnection(_connectionString))
            {
                await conn.OpenAsync();
                post = conn.QueryAsync<News>(sql, new { id = Id }).GetAwaiter().GetResult().FirstOrDefault();
                conn.Close();
            }
            return post;
        }

        public (List<Subscribe>, int) GetListSubscribe(Subscribe request, int pageNumber, int pageSize)
        {
            List<Subscribe> list = new List<Subscribe>();
            int totalRecords = 0;
            var parameters = new DynamicParameters();

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var query = new StringBuilder(@"
                        SELECT * FROM (
                            SELECT ROW_NUMBER() OVER (ORDER BY Id DESC) AS RowNum, 
                                * 
                            FROM Subscribe 
                            WHERE 1 = 1");

                var queryCount = new StringBuilder(@"
                        SELECT COUNT(*) FROM Subscribe WHERE 1=1");

                if (!string.IsNullOrEmpty(request.Email))
                {
                    query.Append(" AND Email LIKE @Email");
                    queryCount.Append(" AND Email LIKE @Email");
                }

                query.Append(@"
                            ) AS Subscribe
                            WHERE RowNum BETWEEN ((@PageNumber - 1) * @PageSize + 1) AND (@PageNumber * @PageSize)
                            ORDER BY RowNum");

                // Tạo đối tượng tham số
                parameters.Add("Email", !string.IsNullOrEmpty(request.Email) ? $"%{request.Email}%" : string.Empty);
                parameters.Add("PageNumber", pageNumber);
                parameters.Add("PageSize", pageSize);

                list = connection.Query<Subscribe>(query.ToString(), parameters).ToList();
                totalRecords = connection.QuerySingle<int>(queryCount.ToString(), parameters);
                connection.Close();
            }
            return (list, totalRecords);
        }

        public async Task<Subscribe> GetSubscribe(int Id)
        {
            Subscribe item = new Subscribe();
            string sql = @"SELECT * FROM Subscribe WHERE Id = @id";
            using (var conn = new SqlConnection(_connectionString))
            {
                await conn.OpenAsync();
                item = conn.QueryAsync<Subscribe>(sql, new { id = Id }).GetAwaiter().GetResult().FirstOrDefault();
                conn.Close();
            }
            return item;
        }

        public async Task<List<Subscribe>> GetSubscribe()
        {
            List<Subscribe> items = new List<Subscribe>();
            string sql = @"SELECT * FROM Subscribe";
            using (var conn = new SqlConnection(_connectionString))
            {
                await conn.OpenAsync();
                items = conn.QueryAsync<Subscribe>(sql).GetAwaiter().GetResult().ToList();
                conn.Close();
            }
            return items;
        }


        public (List<CustomerRequest>, int) GetListCustomerRequest(CustomerRequest request, DateTime fromDate, DateTime toDate, int pageNumber, int pageSize, int IsResolvedValue)
        {
            List<CustomerRequest> list = new List<CustomerRequest>();
            int totalRecords = 0;
            var parameters = new DynamicParameters();

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var query = new StringBuilder(@"
                        SELECT * FROM (
                            SELECT ROW_NUMBER() OVER (ORDER BY Id DESC) AS RowNum, 
                                * 
                            FROM CustomerRequest 
                            WHERE 1 = 1");

                var queryCount = new StringBuilder(@"
                        SELECT COUNT(*) FROM CustomerRequest WHERE 1=1");

                if (!string.IsNullOrEmpty(request.Email))
                {
                    query.Append(" AND Email LIKE @Email");
                    queryCount.Append(" AND Email LIKE @Email");
                }

                if (IsResolvedValue != 999)
                {
                    if (IsResolvedValue == 1)
                    {
                        query.Append(" AND IsResolved = 1");
                        queryCount.Append(" AND IsResolved = 1");
                    }
                    else
                    {
                        query.Append(" AND IsResolved = 0");
                        queryCount.Append(" AND IsResolved = 0");
                    }
                }

                if (fromDate != DateTime.MinValue && toDate != DateTime.MinValue)
                {
                    query.Append(" AND CreatedDate BETWEEN @StartDate AND @EndDate");
                    queryCount.Append(" AND CreatedDate BETWEEN @StartDate AND @EndDate");
                }
                else if (fromDate != DateTime.MinValue && toDate == DateTime.MinValue)
                {
                    query.Append(" AND CreatedDate >= @StartDate");
                    queryCount.Append(" AND CreatedDate >= @StartDate");

                }
                else if (fromDate == DateTime.MinValue && toDate != DateTime.MinValue)
                {
                    query.Append(" AND b.CreatedDate <= @EndDate");
                    queryCount.Append(" AND b.CreatedDate <= @EndDate");

                }


                query.Append(@"
                            ) AS CustomerRequest
                            WHERE RowNum BETWEEN ((@PageNumber - 1) * @PageSize + 1) AND (@PageNumber * @PageSize)
                            ORDER BY RowNum");

                // Tạo đối tượng tham số
                parameters.Add("Email", !string.IsNullOrEmpty(request.Email) ? $"%{request.Email}%" : string.Empty);
                parameters.Add("StartDate", fromDate);
                parameters.Add("EndDate", toDate);
                parameters.Add("PageNumber", pageNumber);
                parameters.Add("PageSize", pageSize);
                list = connection.Query<CustomerRequest>(query.ToString(), parameters).ToList();
                totalRecords = connection.QuerySingle<int>(queryCount.ToString(), parameters);
                connection.Close();
            }
            return (list, totalRecords);
        }

        public async Task<CustomerReViewModel> GetCustomerRequest(int Id)
        {
            CustomerReViewModel item = new CustomerReViewModel();
            string sqlRequest = @"SELECT * FROM CustomerRequest WHERE Id = @Id";
            string sqlResponse = @"SELECT * FROM CustomerResponse WHERE CustomerRequestId = @Id";

            using (var conn = new SqlConnection(_connectionString))
            {
                await conn.OpenAsync();
                item.CustomerRequest = conn.QueryAsync<CustomerRequest>(sqlRequest, new { Id = Id }).GetAwaiter().GetResult().FirstOrDefault();
                item.CustomerResponse = conn.QueryAsync<CustomerResponse>(sqlResponse, new { Id = Id }).GetAwaiter().GetResult().FirstOrDefault();
                conn.Close();
            }
            return item;
        }

        public async Task<List<CompanyHistory>> GetCompanyHistory()
        {
            List<CompanyHistory> list = new List<CompanyHistory>();
            string sql = @"SELECT * FROM CompanyHistory ORDER BY Year DESC";
            using (var conn = new SqlConnection(_connectionString))
            {
                await conn.OpenAsync();
                list = conn.QueryAsync<CompanyHistory>(sql).GetAwaiter().GetResult().ToList();
                conn.Close();
            }
            return list;
        }

        public async Task<CompanyHistory> GetCompanyHistory(int Id)
        {
            CompanyHistory item = new CompanyHistory();
            string sql = @"SELECT * FROM CompanyHistory WHERE Id = @Id";
            using (var conn = new SqlConnection(_connectionString))
            {
                await conn.OpenAsync();
                item = conn.QueryAsync<CompanyHistory>(sql, new {Id = Id}).GetAwaiter().GetResult().FirstOrDefault();
                conn.Close();
            }
            return item;
        }

        public async Task<List<RewardYear>> GetReward()
        {
            List<RewardYear> list = new List<RewardYear>();
            string sql = @"SELECT * FROM RewardYear ORDER BY Year DESC";
            using (var conn = new SqlConnection(_connectionString))
            {
                await conn.OpenAsync();
                list = conn.QueryAsync<RewardYear>(sql).GetAwaiter().GetResult().ToList();
                conn.Close();
            }
            return list;
        }

        public async Task<RewardYear> GetReward(int Id)
        {
            RewardYear item = new RewardYear();
            string sqlYear = @"SELECT * FROM RewardYear WHERE Id = @Id";
            string sqlReward = @"SELECT * FROM Reward WHERE YearId = @Id";

            using (var conn = new SqlConnection(_connectionString))
            {
                await conn.OpenAsync();
                item = conn.QueryAsync<RewardYear>(sqlYear, new { Id = Id }).GetAwaiter().GetResult().FirstOrDefault();
                item.Rewards = conn.QueryAsync<Reward>(sqlReward, new {Id = Id}).GetAwaiter().GetResult().ToList();
                conn.Close();
            }
            return item;
        }



        public (List<Job>, int) GetListJob(Job request, DateTime fromDate, DateTime toDate, int pageNumber, int pageSize)
        {
            List<Job> list = new List<Job>();
            int totalRecords = 0;
            var parameters = new DynamicParameters();

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var query = new StringBuilder(@"
                        SELECT * FROM (
                            SELECT ROW_NUMBER() OVER (ORDER BY Id DESC) AS RowNum, 
                                * 
                            FROM Job 
                            WHERE 1 = 1");

                var queryCount = new StringBuilder(@"
                        SELECT COUNT(*) FROM Job WHERE 1=1");

                if (!string.IsNullOrEmpty(request.Department))
                {
                    query.Append(" AND Department LIKE @Department");
                    queryCount.Append(" AND Department LIKE @Department");
                }

                if (fromDate != DateTime.MinValue && toDate != DateTime.MinValue)
                {
                    query.Append(" AND CreatedDate BETWEEN @StartDate AND @EndDate");
                    queryCount.Append(" AND CreatedDate BETWEEN @StartDate AND @EndDate");
                }
                else if (fromDate != DateTime.MinValue && toDate == DateTime.MinValue)
                {
                    query.Append(" AND CreatedDate >= @StartDate");
                    queryCount.Append(" AND CreatedDate >= @StartDate");

                }
                else if (fromDate == DateTime.MinValue && toDate != DateTime.MinValue)
                {
                    query.Append(" AND CreatedDate <= @EndDate");
                    queryCount.Append(" AND CreatedDate <= @EndDate");
                }


                query.Append(@"
                            ) AS Job
                            WHERE RowNum BETWEEN ((@PageNumber - 1) * @PageSize + 1) AND (@PageNumber * @PageSize)
                            ORDER BY RowNum");

                // Tạo đối tượng tham số
                parameters.Add("Department", !string.IsNullOrEmpty(request.Department) ? $"%{request.Department}%" : string.Empty);
                parameters.Add("StartDate", fromDate);
                parameters.Add("EndDate", toDate);
                parameters.Add("PageNumber", pageNumber);
                parameters.Add("PageSize", pageSize);

                list = connection.Query<Job>(query.ToString(), parameters).ToList();
                totalRecords = connection.QuerySingle<int>(queryCount.ToString(), parameters);
                connection.Close();
            }
            return (list, totalRecords);
        }


        public async Task<List<Job>> GetJob()
        {
            List<Job> list = new List<Job>();
            string sql = @"SELECT * FROM Job ORDER BY CreatedDate DESC";
            using (var conn = new SqlConnection(_connectionString))
            {
                await conn.OpenAsync();
                list = conn.QueryAsync<Job>(sql).GetAwaiter().GetResult().ToList();
                conn.Close();
            }
            return list;
        }
        public async Task<Job> GetJob(int Id)
        {
            Job item = new Job();
            string sqlJob = @"SELECT * FROM Job WHERE Id = @Id";
            string sqlApplication = @"SELECT * FROM Application WHERE JobId = @Id";

            using (var conn = new SqlConnection(_connectionString))
            {
                await conn.OpenAsync();
                item = conn.QueryAsync<Job>(sqlJob, new { Id = Id }).GetAwaiter().GetResult().FirstOrDefault();
                item.ApplicationList = conn.QueryAsync<Application>(sqlApplication, new { Id = Id }).GetAwaiter().GetResult().ToList();
                conn.Close();
            }
            return item;
        }

        #endregion

        #region CREATE

        public async Task<bool> SaveLogs(string logs)
        {
            bool isSuccess = false;
            string SQL = @"INSERT INTO Logs ([Logs]) 
                                VALUES (@logs)";
            int rowAfflect = 0;

            using (var conn = new SqlConnection(_connectionString))
            {
                await conn.OpenAsync();
                rowAfflect = await conn.ExecuteAsync(SQL, logs);
                conn.Close();
                if (rowAfflect > 0)
                {
                    isSuccess = true;
                }
            }
            return isSuccess;
        }

        public async Task<bool> SaveCreateCompanyInfo(CompanyInfo request)
        {
            bool isSuccess = false;
            string SQL = @"INSERT INTO CompanyInfo ([Name], [Description], [Image]) 
                                VALUES (@name, @description, @image)";
            int rowAfflect = 0;

            using (var conn = new SqlConnection(_connectionString))
            {
                await conn.OpenAsync();
                rowAfflect = await conn.ExecuteAsync(SQL, new { image = request.Image, description = request.Description, name = request.Name });
                conn.Close();
                if (rowAfflect > 0)
                {
                    isSuccess = true;
                }
            }
            return isSuccess;
        }

        public async Task<bool> SaveCreatePartnerBanner(PartnerBanner request)
        {
            bool isSuccess = false;
            string SQL = @"INSERT INTO PartnerBanner ([Name], [Image], [IsActived]) 
                                VALUES (@name, @image, 1)";
            int rowAfflect = 0;

            using (var conn = new SqlConnection(_connectionString))
            {
                await conn.OpenAsync();
                rowAfflect = await conn.ExecuteAsync(SQL, new { name = request.Name, image = request.Image });
                conn.Close();
                if (rowAfflect > 0)
                {
                    isSuccess = true;
                }
            }
            return isSuccess;
        }


        public async Task<bool> SaveCreateService(Service request)
        {
            bool isSuccess = false;
            string SQL = @"INSERT INTO Service ([Name], [Image], [ShortDescription], [Link]) 
                                VALUES (@name, @image, @shortDescription, @link)";
            int rowAfflect = 0;

            using (var conn = new SqlConnection(_connectionString))
            {
                await conn.OpenAsync();
                rowAfflect = await conn.ExecuteAsync(SQL, new { name = request.Name, image = request.Image, shortDescription = request.ShortDescription, link = request.Link });
                conn.Close();
                if (rowAfflect > 0)
                {
                    isSuccess = true;
                }
            }
            return isSuccess;
        }

        public async Task<bool> SaveCreateSocialMedia(SocialMedia request)
        {
            bool isSuccess = false;
            string SQL = @"INSERT INTO SocialMedia ([Name], [Image], [Link]) 
                                VALUES (@name, @image, @link)";
            int rowAfflect = 0;

            using (var conn = new SqlConnection(_connectionString))
            {
                await conn.OpenAsync();
                rowAfflect = await conn.ExecuteAsync(SQL, new { name = request.Name, image = request.Image, link = request.Link });
                conn.Close();
                if (rowAfflect > 0)
                {
                    isSuccess = true;
                }
            }
            return isSuccess;
        }

        public async Task<bool> SaveCreateSlider(Slider request)
        {
            bool isSuccess = false;
            string SQL = @"INSERT INTO Slider ([Image], [Position], [Header], [Title], [IsActived]) 
                                VALUES (@image, @position, @header, @title, 1)";
            int rowAfflect = 0;

            using (var conn = new SqlConnection(_connectionString))
            {
                await conn.OpenAsync();
                rowAfflect = await conn.ExecuteAsync(SQL, new { image = request.Image, position = request.Position, header = request.Header, title = request.Title });
                conn.Close();
                if (rowAfflect > 0)
                {
                    isSuccess = true;
                }
            }
            return isSuccess;
        }

        public async Task<bool> SaveCreateCategory(Category request)
        {
            bool isSuccess = false;
            string SQL = @"INSERT INTO Category ([Name], [Alias], [Position], [ParentId], [IsHeaderMenu], [IsActived], [CreatedBy], [CreatedDate]) 
                                VALUES (@name, @alias, @position, @parentId , @isHeaderMenu, 1, @createdBy, @createdDate)";
            int rowAfflect = 0;

            using (var conn = new SqlConnection(_connectionString))
            {
                await conn.OpenAsync();
                rowAfflect = await conn.ExecuteAsync(SQL, new
                {
                    name = request.Name,
                    alias = request.Alias,
                    position = request.Position,
                    parentId = request.ParentId,
                    isHeaderMenu = request.IsHeaderMenu,
                    createdBy = request.CreatedBy,
                    createdDate = request.CreatedDate
                });

                conn.Close();
                if (rowAfflect > 0)
                {
                    isSuccess = true;
                }
            }
            return isSuccess;
        }

        public async Task<bool> SaveCreatePost(Post request)
        {
            bool isSuccess = false;
            string SQL = @"INSERT INTO Post ([Name], [Alias], [CategoryId], [ShortDescription], [LongDescription], [ViewCount], [IsActived], [CreatedBy], [CreatedDate]) 
                                VALUES (@name, @alias, @categoryId, @shortDescription, @longDescription, 0, 1, @createdBy, @createdDate)";
            int rowAfflect = 0;

            using (var conn = new SqlConnection(_connectionString))
            {
                await conn.OpenAsync();
                rowAfflect = await conn.ExecuteAsync(SQL, new
                {
                    name = request.Name,
                    alias = request.Alias,
                    categoryId = request.CategoryId,
                    shortDescription = request.ShortDescription,
                    longDescription = request.LongDescription,
                    createdBy = request.CreatedBy,
                    createdDate = request.CreatedDate
                });

                conn.Close();
                if (rowAfflect > 0)
                {
                    isSuccess = true;
                }
            }
            return isSuccess;
        }

        public async Task<bool> SaveCreateNews(News request)
        {
            bool isSuccess = false;
            string SQL = @"INSERT INTO News ([Name], [Alias], [Image] , [ShortDescription], [LongDescription], [ViewCount], [IsActived], [CreatedBy], [CreatedDate]) 
                                VALUES (@name, @alias, @image, @shortDescription, @longDescription, 0, 1, @createdBy, @createdDate)";
            int rowAfflect = 0;

            using (var conn = new SqlConnection(_connectionString))
            {
                await conn.OpenAsync();
                rowAfflect = await conn.ExecuteAsync(SQL, new
                {
                    name = request.Name,
                    alias = request.Alias,
                    image = request.Image,
                    shortDescription = request.ShortDescription,
                    longDescription = request.LongDescription,
                    createdBy = request.CreatedBy,
                    createdDate = request.CreatedDate
                });

                conn.Close();
                if (rowAfflect > 0)
                {
                    isSuccess = true;
                }
            }
            return isSuccess;
        }

        public async Task<bool> SaveNotificationContent(Notification request)
        {
            bool isSuccess = false;
            string SQL = @"INSERT INTO Notification ([Title], [Body], [CreatedBy], [CreatedDate]) 
                                VALUES (@title, @body, @createdBy, @createdDate)";
            int rowAfflect = 0;

            using (var conn = new SqlConnection(_connectionString))
            {
                await conn.OpenAsync();
                rowAfflect = await conn.ExecuteAsync(SQL, new
                {
                    title = request.Title,
                    body = request.Body,
                    createdBy = request.CreatedBy,
                    createdDate = request.CreatedDate
                });

                conn.Close();
                if (rowAfflect > 0)
                {
                    isSuccess = true;
                }
            }
            return isSuccess;
        }
        public async Task<bool> SaveCustomerResponse(CustomerResponse request)
        {
            bool isSuccess = false;
            string SQL = @"INSERT INTO CustomerResponse ( [CustomerRequestId], [Subject], [Description], [CreatedBy], [CreatedDate]) 
                                VALUES ( @customerRequestId, @subject, @description, @createdBy, @createdDate)";

            string SQLUpdateRequest = @"
                            UPDATE CustomerRequest 
                            SET IsResolved = 1 
                            WHERE Id = @Id";

            int rowAfflect = 0;

            using (var conn = new SqlConnection(_connectionString))
            {
                await conn.OpenAsync();
                rowAfflect = await conn.ExecuteAsync(SQL, new
                {
                    customerRequestId = request.CustomerRequestId,
                    subject = request.Subject,
                    description = request.Description,
                    createdBy = request.CreatedBy,
                    createdDate = request.CreatedDate
                });
                rowAfflect = await conn.ExecuteAsync(SQLUpdateRequest, new
                {
                    id = request.CustomerRequestId,
                });
                if (rowAfflect > 0)
                {
                    isSuccess = true;
                }
                conn.Close();

            }
            return isSuccess;
        }

        public async Task<bool> SaveCreateCompanyHistory(CompanyHistory request)
        {
            bool isSuccess = false;
            string SQL = @"INSERT INTO CompanyHistory ([Year], [Event], [Image]) 
                                VALUES (@year, @eventString, @image)";
            int rowAfflect = 0;

            using (var conn = new SqlConnection(_connectionString))
            {
                await conn.OpenAsync();
                rowAfflect = await conn.ExecuteAsync(SQL, new
                {
                    year = request.Year,
                    eventString = request.Event,
                    image = request.Image,
                });

                conn.Close();
                if (rowAfflect > 0)
                {
                    isSuccess = true;
                }
            }
            return isSuccess;
        }

        public async Task<bool> SaveCreateReward(RewardYear request)
        {
            string sqlYear = @"INSERT INTO RewardYear ([Year]) 
                                OUTPUT INSERTED.ID
                                VALUES (@year)";

            string sqlReward = @"
                                 INSERT INTO Reward ([YearId], [RewardName], [RewardFrom])
                                    VALUES (@yearId, @rewardName, @rewardFrom)
                                ";
            int ID = 0;
            int x = 0;
            using (var conn = new SqlConnection(_connectionString))
            {
                await conn.OpenAsync();
                using (var transaction = conn.BeginTransaction())
                {
                    try
                    {
                        ID = conn.QuerySingleAsync<int>(sqlYear, new
                        {
                           year = request.Year
                        }, transaction, commandTimeout: 30, commandType: CommandType.Text).GetAwaiter().GetResult();

                        if (ID > 0)
                        {
                            for (int i = 0; i < request.Rewards.Count; i++)
                            {
                                x = conn.ExecuteAsync(sqlReward, new
                                {
                                    yearId = ID,
                                    rewardName = request.Rewards[i].RewardName,
                                    rewardFrom = request.Rewards[i].RewardFrom
                                }, transaction, commandTimeout: 30, commandType: CommandType.Text).GetAwaiter().GetResult();
                            }
                            
                            if (x > 0)
                            {
                                transaction.Commit();
                                return true;
                            }
                        }
                        transaction.Commit();
                        return true;
                    }
                    catch
                    {
                        transaction.Rollback();
                        return false;
                    }
                }
            }
        }

        public async Task<bool> SaveCreateJob(Job request)
        {
            bool isSuccess = false;
            string SQL = @"INSERT INTO Job ([Name], [Location], [Department], [FromSalary], [ToSalary], [IsDeal], [Description], [Requirement], [Benefit], [IsActived], [CreatedDate], [OpenDate], [CloseDate]) 
                                VALUES (@name, @location, @department, @fromSalary, @toSalary, @isDeal, @description, @requirement, @benefit , @isActived, @createdDate, @openDate, @closeDate)";
            int rowAfflect = 0;

            using (var conn = new SqlConnection(_connectionString))
            {
                await conn.OpenAsync();
                rowAfflect = await conn.ExecuteAsync(SQL,  new 
                {
                    name = request.Name,
                    location = request.Location,
                    department = request.Department,
                    fromSalary = request.FromSalary,
                    toSalary = request.ToSalary,
                    isDeal = request.IsDeal,
                    description = request.Description,
                    requirement = request.Requirement,
                    benefit = request.Benefit,
                    isActived = true,
                    createdDate = request.CreatedDate,
                    openDate = request.OpenDate,
                    closeDate = request.CloseDate,
                });
                conn.Close();
                if (rowAfflect > 0)
                {
                    isSuccess = true;
                }
            }
            return isSuccess;
        }

        #endregion


        #region EDIT
        public async Task<bool> UpdateLogo(Logo request)
        {
            bool isSuccess = false;
            string SQL = @"UPDATE Logo 
                            SET Image = @image, 
                                Width = @width,
                                Height = @height
                            WHERE Id = @Id";
            int rowAfflect = 0;

            using (var conn = new SqlConnection(_connectionString))
            {
                await conn.OpenAsync();
                rowAfflect = await conn.ExecuteAsync(SQL, new { image = request.Image, width = request.Width, height = request.Height, Id = request.Id });
                conn.Close();
                if (rowAfflect > 0)
                {
                    isSuccess = true;
                }
            }
            return isSuccess;
        }
        public async Task<bool> SaveEditCompanyInfo(CompanyInfo request)
        {
            bool isSuccess = false;
            string SQL = @"UPDATE CompanyInfo 
                            SET Name = @name, 
                                Description = @description,
                                Image = @image 
                            WHERE Id = @id";
            int rowAfflect = 0;

            using (var conn = new SqlConnection(_connectionString))
            {
                await conn.OpenAsync();
                rowAfflect = await conn.ExecuteAsync(SQL, new { image = request.Image, description = request.Description, name = request.Name, id = request.Id });
                conn.Close();
                if (rowAfflect > 0)
                {
                    isSuccess = true;
                }
            }
            return isSuccess;
        }

        public async Task<bool> SaveEditPartnerBanner(PartnerBanner request)
        {
            bool isSuccess = false;
            string SQL = @"UPDATE PartnerBanner 
                            SET Name = @name, 
                                Image = @image,
                                IsActived = 1 
                            WHERE Id = @id";
            int rowAfflect = 0;

            using (var conn = new SqlConnection(_connectionString))
            {
                await conn.OpenAsync();
                rowAfflect = await conn.ExecuteAsync(SQL, new { image = request.Image, name = request.Name, id = request.Id });
                conn.Close();
                if (rowAfflect > 0)
                {
                    isSuccess = true;
                }
            }
            return isSuccess;
        }

        public async Task<bool> SaveEditService(Service request)
        {
            bool isSuccess = false;
            string SQL = @"UPDATE Service  
                            SET Name = @name, 
                                Image = @image,
                                ShortDescription = @shortDescription,
                                Link = @link 
                            WHERE Id = @id";
            int rowAfflect = 0;

            using (var conn = new SqlConnection(_connectionString))
            {
                await conn.OpenAsync();
                rowAfflect = await conn.ExecuteAsync(SQL, new { image = request.Image, name = request.Name, shortDescription = request.ShortDescription, link = request.Link, id = request.Id });
                conn.Close();
                if (rowAfflect > 0)
                {
                    isSuccess = true;
                }
            }
            return isSuccess;
        }

        public async Task<bool> SaveEditSocialMedia(SocialMedia request)
        {
            bool isSuccess = false;
            string SQL = @"UPDATE SocialMedia  
                            SET Name = @name, 
                                Image = @image,
                                Link = @link 
                            WHERE Id = @id";
            int rowAfflect = 0;

            using (var conn = new SqlConnection(_connectionString))
            {
                await conn.OpenAsync();
                rowAfflect = await conn.ExecuteAsync(SQL, new { image = request.Image, name = request.Name, link = request.Link, id = request.Id });
                conn.Close();
                if (rowAfflect > 0)
                {
                    isSuccess = true;
                }
            }
            return isSuccess;
        }

        public async Task<bool> SaveEditSlider(Slider request)
        {
            bool isSuccess = false;
            string SQL = @"UPDATE Slider  
                            SET Image = @image,
                                Position = @position,
                                Header = @header, 
                                Title = @title,
                                IsActived = 1 
                            WHERE Id = @id";
            int rowAfflect = 0;

            using (var conn = new SqlConnection(_connectionString))
            {
                await conn.OpenAsync();
                rowAfflect = await conn.ExecuteAsync(SQL, new { image = request.Image, position = request.Position, header = request.Header, title = request.Title, id = request.Id });
                conn.Close();
                if (rowAfflect > 0)
                {
                    isSuccess = true;
                }
            }
            return isSuccess;
        }

        public async Task<bool> SaveEditCategory(Category request)
        {
            bool isSuccess = false;
            string SQL = @"UPDATE Category   
                            SET Name = @name,
                                Position = @position, 
                                ParentId = @parentId,
                                IsHeaderMenu = @isHeaderMenu,
                                ModifiedBy = @modifiedBy, 
                                ModifiedDate = @modifiedDate, 
                                IsActived = 1 
                            WHERE Id = @id";
            int rowAfflect = 0;

            using (var conn = new SqlConnection(_connectionString))
            {
                await conn.OpenAsync();
                rowAfflect = await conn.ExecuteAsync(SQL, new
                {
                    name = request.Name,
                    position = request.Position,
                    parentId = request.ParentId,
                    isHeaderMenu = request.IsHeaderMenu,
                    modifiedBy = request.ModifiedBy,
                    modifiedDate = request.ModifiedDate,
                    id = request.Id
                });
                conn.Close();
                if (rowAfflect > 0)
                {
                    isSuccess = true;
                }
            }
            return isSuccess;
        }

        public async Task<bool> SaveEditPost(Post request)
        {
            bool isSuccess = false;
            string SQL = @"UPDATE Post   
                            SET Name = @name,
                                Alias = @alias,
                                ShortDescription = @shortDescription, 
                                LongDescription = @longDescription, 
                                IsActived = 1,
                                ModifiedBy = @modifiedBy, 
                                ModifiedDate = @modifiedDate 
                            WHERE Id = @id";
            int rowAfflect = 0;

            using (var conn = new SqlConnection(_connectionString))
            {
                await conn.OpenAsync();
                rowAfflect = await conn.ExecuteAsync(SQL, new
                {
                    name = request.Name,
                    alias = request.Alias,
                    categoryId = request.CategoryId,
                    shortDescription = request.ShortDescription,
                    longDescription = request.LongDescription,
                    modifiedBy = request.ModifiedBy,
                    modifiedDate = request.ModifiedDate,
                    id = request.Id
                });
                conn.Close();
                if (rowAfflect > 0)
                {
                    isSuccess = true;
                }
            }
            return isSuccess;
        }

        public async Task<bool> SaveEditNews(News request)
        {
            bool isSuccess = false;
            string SQL = @"UPDATE News   
                            SET Name = @name,
                                Alias = @alias,
                                Image = @image,
                                ShortDescription = @shortDescription, 
                                LongDescription = @longDescription, 
                                IsActived = 1,
                                ModifiedBy = @modifiedBy, 
                                ModifiedDate = @modifiedDate 
                            WHERE Id = @id";
            int rowAfflect = 0;

            using (var conn = new SqlConnection(_connectionString))
            {
                await conn.OpenAsync();
                rowAfflect = await conn.ExecuteAsync(SQL, new
                {
                    name = request.Name,
                    alias = request.Alias,
                    image = request.Image,
                    shortDescription = request.ShortDescription,
                    longDescription = request.LongDescription,
                    modifiedBy = request.ModifiedBy,
                    modifiedDate = request.ModifiedDate,
                    id = request.Id
                });
                conn.Close();
                if (rowAfflect > 0)
                {
                    isSuccess = true;
                }
            }
            return isSuccess;
        }

        public async Task<bool> SaveEditCompanyHistory(CompanyHistory request)
        {
            bool isSuccess = false;
            string SQL = @"UPDATE CompanyHistory   
                            SET Year = @year,
                                Event = @eventString, 
                                Image = @image
                            WHERE Id = @id";
            int rowAfflect = 0;

            using (var conn = new SqlConnection(_connectionString))
            {
                await conn.OpenAsync();
                rowAfflect = await conn.ExecuteAsync(SQL, new
                {
                    year = request.Year,
                    eventString = request.Event,
                    image = request.Image,
                    id = request.Id
                });
                conn.Close();
                if (rowAfflect > 0)
                {
                    isSuccess = true;
                }
            }
            return isSuccess;
        }
        public async Task<bool> SaveEditReward(List<Reward> request, int yearId)
        {
            bool isSuccess = false;

            string SqlDelete = @"DELETE FROM Reward WHERE YearId = @yearId";

            string SQLInsert = @"INSERT INTO Reward ([YearId], [RewardName], [RewardFrom]) 
                                    VALUES (@yearId, @rewardName, @rewardFrom) ";

            int deleteAfflect = 0;

            int rowAfflect = 0;

            using (var conn = new SqlConnection(_connectionString))
            {
                await conn.OpenAsync();
                deleteAfflect = await conn.ExecuteAsync(SqlDelete, new { yearId = yearId});
                foreach (var item in request)
                {
                    rowAfflect = await conn.ExecuteAsync(SQLInsert, new
                    {
                        yearId = yearId,
                        rewardName = item.RewardName,
                        rewardFrom = item.RewardFrom
                    });
                }
              
                conn.Close();
                if (rowAfflect > 0)
                {
                    isSuccess = true;
                }
            }
            return isSuccess;
        }


        public async Task<bool> SaveEditJob(Job request)
        {
            bool isSuccess = false;
            string SQL = @"UPDATE Job   
                            SET Name = @name,
                                Location = @location, 
                                Department = @department,
                                FromSalary = @fromSalary, 
                                ToSalary = @toSalary, 
                                IsDeal = @isDeal,
                                Description = @description, 
                                Requirement = @requirement, 
                                Benefit = @benefit, 
                                IsActived = @isActived,
                                OpenDate = @openDate, 
                                CloseDate = @closeDate
    
                            WHERE Id = @id";
            int rowAfflect = 0;

            using (var conn = new SqlConnection(_connectionString))
            {
                await conn.OpenAsync();
                rowAfflect = await conn.ExecuteAsync(SQL, new
                {
                    name = request.Name,
                    location = request.Location,
                    department = request.Department,
                    fromSalary = request.FromSalary,
                    toSalary = request.ToSalary,
                    isDeal = request.IsDeal,
                    description = request.Description,
                    requirement = request.Requirement,
                    benefit = request.Benefit,
                    isActived = true,
                    openDate = request.OpenDate,
                    closeDate = request.CloseDate,
                    id = request.Id
                });
                conn.Close();
                if (rowAfflect > 0)
                {
                    isSuccess = true;
                }
            }
            return isSuccess;
        }

        #endregion

        #region ISACTIVE
        public async Task<bool> ChangeActivePartnerBanner(int Id, bool isActive)
        {
            int x = 0;
            string sql = @"UPDATE [PartnerBanner] SET [IsActived] = @isActive where Id = @Id ";
            using (var conn = new SqlConnection(_connectionString))
            {
                await conn.OpenAsync();
                x = await conn.ExecuteAsync(sql, new { isActive, Id }, null, commandTimeout: 30, commandType: CommandType.Text);
                conn.Dispose();
            }
            if (x > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        public async Task<bool> ChangeActiveSlider(int Id, bool isActive)
        {
            int x = 0;
            string sql = @"UPDATE [Slider] SET [IsActived] = @isActive where Id = @Id ";
            using (var conn = new SqlConnection(_connectionString))
            {
                await conn.OpenAsync();
                x = await conn.ExecuteAsync(sql, new { isActive, Id }, null, commandTimeout: 30, commandType: CommandType.Text);
                conn.Dispose();
            }
            if (x > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<bool> ChangeActiveCategory(int Id, bool isActive)
        {
            int x = 0;
            string sql = @"UPDATE [Category] SET [IsActived] = @isActive where Id = @Id ";
            using (var conn = new SqlConnection(_connectionString))
            {
                await conn.OpenAsync();
                x = await conn.ExecuteAsync(sql, new { isActive, Id }, null, commandTimeout: 30, commandType: CommandType.Text);
                conn.Dispose();
            }
            if (x > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<bool> ChangeActiveCategoryHeaderMenu(int Id, bool isActive)
        {
            int x = 0;
            string sql = @"UPDATE [Category] SET [IsHeaderMenu] = @isActive where Id = @Id ";
            using (var conn = new SqlConnection(_connectionString))
            {
                await conn.OpenAsync();
                x = await conn.ExecuteAsync(sql, new { isActive, Id }, null, commandTimeout: 30, commandType: CommandType.Text);
                conn.Dispose();
            }
            if (x > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<bool> ChangeActivePost(int Id, bool isActive)
        {
            int x = 0;
            string sql = @"UPDATE [Post] SET [IsActived] = @isActive where Id = @Id ";
            using (var conn = new SqlConnection(_connectionString))
            {
                await conn.OpenAsync();
                x = await conn.ExecuteAsync(sql, new { isActive, Id }, null, commandTimeout: 30, commandType: CommandType.Text);
                conn.Dispose();
            }
            if (x > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public async Task<bool> ChangeActiveNews(int Id, bool isActive)
        {
            int x = 0;
            string sql = @"UPDATE [News] SET [IsActived] = @isActive where Id = @Id ";
            using (var conn = new SqlConnection(_connectionString))
            {
                await conn.OpenAsync();
                x = await conn.ExecuteAsync(sql, new { isActive, Id }, null, commandTimeout: 30, commandType: CommandType.Text);
                conn.Dispose();
            }
            if (x > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public async Task<bool> ChangeActiveJob(int Id, bool isActive)
        {
            int x = 0;
            string sql = @"UPDATE [Job] SET [IsActived] = @isActive where Id = @Id ";
            using (var conn = new SqlConnection(_connectionString))
            {
                await conn.OpenAsync();
                x = await conn.ExecuteAsync(sql, new { isActive, Id }, null, commandTimeout: 30, commandType: CommandType.Text);
                conn.Dispose();
            }
            if (x > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task ChangeActiveJob()
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                await conn.OpenAsync();
                await conn.ExecuteAsync("UpdateJobStatus", commandType: System.Data.CommandType.StoredProcedure);
                conn.Dispose();
            }
        }


        #endregion

        #region DELETE

        public async Task<bool> DeleteService(int id)
        {
            int x = 0;
            string sql = @"DELETE FROM [Service] WHERE Id = @Id ";
            using (var conn = new SqlConnection(_connectionString))
            {
                await conn.OpenAsync();
                x = await conn.ExecuteAsync(sql, new { Id = id }, null, commandTimeout: 30, commandType: CommandType.Text);
                conn.Dispose();
            }
            if (x > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<bool> DeleteSocialMedia(int id)
        {
            int x = 0;
            string sql = @"DELETE FROM [SocialMedia] WHERE Id = @Id ";
            using (var conn = new SqlConnection(_connectionString))
            {
                await conn.OpenAsync();
                x = await conn.ExecuteAsync(sql, new { Id = id }, null, commandTimeout: 30, commandType: CommandType.Text);
                conn.Dispose();
            }
            if (x > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<bool> DeleteSlider(int id)
        {
            int x = 0;
            string sql = @"DELETE FROM [Slider] WHERE Id = @Id ";
            using (var conn = new SqlConnection(_connectionString))
            {
                await conn.OpenAsync();
                x = await conn.ExecuteAsync(sql, new { Id = id }, null, commandTimeout: 30, commandType: CommandType.Text);
                conn.Dispose();
            }
            if (x > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        #endregion

        #region OTHER

        public Task<bool> SendMailCustomerResponse(CustomerResponse request, string customerEmail)
        {
            bool isSuccess = false;
            isSuccess = Manager.Common.Helpers.Common.SendMail("ENVIET GROUP", request.Subject, request.Description, customerEmail, "noreply.enviet@airline24h.vn", "/mqPQu8uqWecyBYh+2jVbg==", "smtp.gmail.com", 587, true, "", "", false, false);
            return Task.FromResult(isSuccess);
        }


        /// <summary>
        /// 1 giờ gửi tối đa 50 email
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task SendMailToSubscribers(Notification request)
        {
            var subscribers = await GetSubscribe();
            int batchSize = 30; // Số lượng email mỗi lô
            int maxEmailsPerMinute = 30; // Giới hạn số email gửi mỗi phút

            for (int i = 0; i < subscribers.Count; i += batchSize)
            {
                var batch = subscribers.Skip(i).Take(batchSize).ToList();
                var emailTasks = new List<Task<bool>>(); // Danh sách tác vụ trả về bool
                var subscriberEmails = new List<string>(); // Danh sách lưu trữ email

                foreach (var subscriber in batch)
                {
                    subscriberEmails.Add(subscriber.Email); // Lưu email
                    emailTasks.Add(Task.Run(() =>
                        Manager.Common.Helpers.Common.SendMail("ENVIET GROUP", request.Title, request.Body,
                        subscriber.Email, "noreply.enviet@airline24h.vn", "/mqPQu8uqWecyBYh+2jVbg==",
                        "smtp.gmail.com", 587, true, "", "", false, false)));
                }

                // Chờ cho tất cả các email trong lô này được gửi
                var results = await Task.WhenAll(emailTasks);

                // Ghi lại các email không gửi thành công
                List<string> failedEmails = new List<string>();

                for (int j = 0; j < results.Length; j++)
                {
                    if (!results[j]) // Nếu kết quả trả về false, nghĩa là gửi không thành công
                    {
                        failedEmails.Add(subscriberEmails[j]); // Thêm email không gửi được
                    }
                }

                if (failedEmails.Any())
                {
                    _ = Task.Run(() => SaveLogs("Notificate: Các email không gửi thành công trong lô: " + string.Join(", ", failedEmails)));
                }

                // Nếu số lượng email gửi đã đạt đến giới hạn, chờ 1 phút trước khi tiếp tục
                if (i + batchSize >= maxEmailsPerMinute)
                {
                    //await Task.Delay(60000); // Chờ 1 phút
                    await Task.Delay(3600000); // Chờ 1 giờ (1 giờ = 60 phút = 3600 giây = 3600000 mili giây)
                }
            }
        }




        #endregion



    }
}
