using Manager.DataAccess.IRepository.IUnitOfWork_Repository;
using Manager.DataAccess.Repository;
using Manager.DataAccess.Services.Sinhnhatkhachvip;
using Manager.Model.Services.Abstraction;
using Microsoft.Extensions.Configuration;
using System;
using System.Net.Http;

namespace Manager.DataAccess
{
    public class UnitOfWork_Repository : IUnitOfWork_Repository
    {
        #region Biến Lấy từ Container của DI
        private readonly IConfiguration _configuration;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IAuthenticateService _authenticateService;
        private readonly INotifyService _notifyService;
        private readonly SinhnhatDbContext _sinhnhatDbContext;
        #endregion

        public UnitOfWork_Repository(IConfiguration configuration, IHttpClientFactory httpClientFactory, IAuthenticateService authenticateService, INotifyService notifyService,
              SinhnhatDbContext sinhnhatDbContext)
        {
            _configuration = configuration;
            _httpClientFactory = httpClientFactory;
            _authenticateService = authenticateService;
            _notifyService = notifyService;
            _sinhnhatDbContext = sinhnhatDbContext;
            
        }

        // tham số ref dùng để lấy lun tham chiếu của biến chứ không phải chỉ là giá trị
        private T GetOrCreate<T>(ref T field, Func<T> delegateFactory) where T : class
        {
            if (field == null) // Nếu field = null thì mới khởi tạo
            {
                field = delegateFactory();
            }
            return field;
        }

        #region KEY_WORD: A
        private ArticleRepository _articleRepository;
        public ArticleRepository Article_Rep
        {
            get => GetOrCreate(ref _articleRepository, () => new ArticleRepository(_configuration));
        }
        #endregion

        #region KEY_WORD: B
        private BangTinRepository _bangTinRepository;
        private BankRepository _bankRepository;
        private BankStatementRepository _bankStatementRepository;
        private BienDongSoDuRepository _bienDongSoDuRepository;
        private BookerClubRepository _bookerClubRepository;
        private BookingRepository _bookingRepository;

        public BangTinRepository BangTin_Rep
        {
            get => GetOrCreate(ref _bangTinRepository, () => new BangTinRepository(_configuration));
        }
        public BankRepository Bank_Rep
        {
            get => GetOrCreate(ref _bankRepository, () => new BankRepository(_configuration));
        }
        public BankStatementRepository BankStatement_Rep
        {
            get => GetOrCreate(ref _bankStatementRepository, () => new BankStatementRepository(_configuration));
        }
        public BienDongSoDuRepository BienDongSoDu_Rep
        {
            get => GetOrCreate(ref _bienDongSoDuRepository, () => new BienDongSoDuRepository());
        }
        public BookerClubRepository BookerClub_Rep
        {
            get => GetOrCreate(ref _bookerClubRepository, () => new BookerClubRepository(_configuration));
        }
        public BookingRepository Booking_Rep
        {
            get => GetOrCreate(ref _bookingRepository, () => new BookingRepository(_configuration));
        }
        #endregion

        #region KEY_WORD: C
        private ChartRepository _chartRepository;
        private ChuongTrinhKhuyenMaiRepository _chuongTrinhKhuyenMaiRepository;
        private ChuongTrinhXoSoRepository _chuongTrinhXoSoRepository;
        private ConfigBalanceAirlinesRepository _configBalanceAirlinesRepository;
        private ConfigPhiXuatRepository _configPhiXuatRepository;
        private CongNoQuaHanRepository _congNoQuaHanRepository;
        private CongNoRepository _congNoRepository;

        public ChartRepository Chart_Rep
        {
            get => GetOrCreate(ref _chartRepository, () => new ChartRepository());
        }
        public ChuongTrinhKhuyenMaiRepository CTKM_Rep
        {
            get => GetOrCreate(ref _chuongTrinhKhuyenMaiRepository, () => new ChuongTrinhKhuyenMaiRepository(_configuration));
        }
        public ChuongTrinhXoSoRepository CTXS_Rep
        {
            get => GetOrCreate(ref _chuongTrinhXoSoRepository, () => new ChuongTrinhXoSoRepository(_configuration));
        }
        public ConfigBalanceAirlinesRepository ConfigBalanceAirlines_Rep
        {
            get => GetOrCreate(ref _configBalanceAirlinesRepository, () => new ConfigBalanceAirlinesRepository(_configuration));
        }
        public ConfigPhiXuatRepository ConfigPhiXuat_Rep
        {
            get => GetOrCreate(ref _configPhiXuatRepository, () => new ConfigPhiXuatRepository(_configuration));
        }
        public CongNoQuaHanRepository CongNoQuaHan_Rep
        {
            get => GetOrCreate(ref _congNoQuaHanRepository, () => new CongNoQuaHanRepository(_configuration));
        }
        public CongNoRepository CongNo_Rep
        {
            get => GetOrCreate(ref _congNoRepository, () => new CongNoRepository(_configuration));
        }
        #endregion

        #region KEY_WORD: D
        private DaiLyRepository _daiLyRespository;
        private DecentralizationRepository _decentralizationRepository;
        private DinhDanhBaoLuuRepository _dinhDanhBaoLuuRepository;
        private DoanhSoHangRepository _doanhSoHangRepository;
        private DulichRepository _dulichRepository;

        public DaiLyRepository DaiLy_Rep
        {
            get => GetOrCreate(ref _daiLyRespository, () => new DaiLyRepository(_configuration));
        }
        public DecentralizationRepository Decentralization_Rep
        {
            get => GetOrCreate(ref _decentralizationRepository, () => new DecentralizationRepository());
        }
        public DinhDanhBaoLuuRepository DinhDanhBaoLuu_Rep
        {
            get => GetOrCreate(ref _dinhDanhBaoLuuRepository, () => new DinhDanhBaoLuuRepository());
        }
        public DoanhSoHangRepository DoanhSoHang_Rep
        {
            get => GetOrCreate(ref _doanhSoHangRepository, () => new DoanhSoHangRepository(_configuration));
        }
        public DulichRepository Dulich_Rep
        {
            get => GetOrCreate(ref _dulichRepository, () => new DulichRepository(_configuration));
        }

        #endregion

        #region KEY_WORD: E
        private EVMailRepository _eVMailRepository;
        private EmployeeRepository _employeeRepository;

        public EVMailRepository EVMail_Rep
        {
            get => GetOrCreate(ref _eVMailRepository, () => new EVMailRepository(_configuration));
        }
        public EmployeeRepository Employee_Rep
        {
            get => GetOrCreate(ref _employeeRepository, () => new EmployeeRepository(_configuration));
        }

        #endregion

        #region KEY_WORD: F
        private FareRulesRepository _fareRulesRepository;
        private FlagRepository _flagRepository;
        private FlightGroupRepository _flightGroupRepository;
        private FlightRepository _flightRepository;
        private ForumRepository _forumRepository;

        public FareRulesRepository FareRules_Rep
        {
            get => GetOrCreate(ref _fareRulesRepository, () => new FareRulesRepository(_configuration));
        }
        public FlagRepository Flag_Rep
        {
            get => GetOrCreate(ref _flagRepository, () => new FlagRepository(_configuration));
        }
        public FlightGroupRepository FlightGroup_Rep
        {
            get => GetOrCreate(ref _flightGroupRepository, () => new FlightGroupRepository(_configuration));
        }
        public FlightRepository Flight_Rep
        {
            get => GetOrCreate(ref _flightRepository, () => new FlightRepository(_configuration));
        }
        public ForumRepository Forum_Rep
        {
            get => GetOrCreate(ref _forumRepository, () => new ForumRepository(_configuration));
        }
        #endregion

        #region KEY_WORD: G
        private GatewayRepository _gatewayRepository;
        private GuiMailDaiLyRepository _guiMailDaiLyRepository;

        public GatewayRepository Gateway_Rep
        {
            get => GetOrCreate(ref _gatewayRepository, () => new GatewayRepository(_configuration));
        }
        public GuiMailDaiLyRepository GuiMail_DaiLy_Rep
        {
            get => GetOrCreate(ref _guiMailDaiLyRepository, () => new GuiMailDaiLyRepository(_configuration));
        }

        #endregion

        #region KEY_WORD: H
        private HangRepository _hangRepository;
        private HotelRepository _hotelRepository;

        public HangRepository Hang_Rep
        {
            get => GetOrCreate(ref _hangRepository, () => new HangRepository(_configuration, _httpClientFactory, _authenticateService, _notifyService));
        }
        public HotelRepository Hotel_Rep
        {
            get => GetOrCreate(ref _hotelRepository, () => new HotelRepository(_configuration));
        }
        #endregion

        #region KEY_WORD: I
        private ImportDoanhSoRepository _importDoanhSoRepository;
        private IPRepository _ipRepository;

        public ImportDoanhSoRepository ImportDoanhSo_Rep
        {
            get => GetOrCreate(ref _importDoanhSoRepository, () => new ImportDoanhSoRepository());
        }

        public IPRepository IP_Rep
        {
            get => GetOrCreate(ref _ipRepository, () => new IPRepository(_configuration));
        }
        #endregion

        #region KEY_WORD: K
        private KhachHangRepository _khachHangRepository;
        private KhoaCodeDaiLyRepository _khoaCodeDaiLyRepository;
        private KhoaHoaDonRepository _khoaHoaDonRepository;
        private KyNiemRepository _kyNiemRepository;

        public KhachHangRepository KhachHang_Rep
        {
            get => GetOrCreate(ref _khachHangRepository, () => new KhachHangRepository(_configuration));
        }
        public KhoaCodeDaiLyRepository KhoaCodeDaiLy_Rep
        {
            get => GetOrCreate(ref _khoaCodeDaiLyRepository, () => new KhoaCodeDaiLyRepository(_configuration));
        }
        public KhoaHoaDonRepository KhoaHoaDon_Rep
        {
            get => GetOrCreate(ref _khoaHoaDonRepository, () => new KhoaHoaDonRepository());
        }
        public KyNiemRepository KyNiem_Rep
        {
            get => GetOrCreate(ref _kyNiemRepository, () => new KyNiemRepository());
        }
        #endregion

        #region KEY_WORD: L
        private LichLamViecRepository _lichLamViecRepository;
        private LocationRepository _locationRepository;
        private Log_SendSMSRepository _logSendSMSRepository;
        private LoginRepository _loginRepository;
        private LotusmileRepository _lotusmileRepository;
        private LandingPageRepository _landingPageRepository;

        public LichLamViecRepository LichLamViec_Rep
        {
            get => GetOrCreate(ref _lichLamViecRepository, () => new LichLamViecRepository());
        }
        public LocationRepository Location_Rep
        {
            get => GetOrCreate(ref _locationRepository, () => new LocationRepository(_configuration));
        }
        public Log_SendSMSRepository Log_SendSMS_Rep
        {
            get => GetOrCreate(ref _logSendSMSRepository, () => new Log_SendSMSRepository(_configuration));
        }
        public LoginRepository Login_Rep
        {
            get => GetOrCreate(ref _loginRepository, () => new LoginRepository(_configuration));
        }
        public LotusmileRepository Lotusmile_Rep
        {
            get => GetOrCreate(ref _lotusmileRepository, () => new LotusmileRepository(_configuration));
        }
        public LandingPageRepository LandingPage_Rep
        {
            get => GetOrCreate(ref _landingPageRepository, () => new LandingPageRepository(_configuration));
        }
        #endregion

        #region KEY_WORD: M
        private MemberRepository _memberRepository;
        public MemberRepository Member_Rep
        {
            get => GetOrCreate(ref _memberRepository, () => new MemberRepository());
        }
        #endregion

        #region KEY_WORD: N
        private NotifyRepository _notifyRepository;
        public NotifyRepository Notify_Rep
        {
            get => GetOrCreate(ref _notifyRepository, () => new NotifyRepository(_configuration));
        }
        #endregion

        #region KEY_WORD: P
        private PermissionRepository _permissionRepository;
        private PhanViecRepository _phanViecRepository;
        private PhieuBaoLanhRepository _phieuBaoLanhRepository;
        private PopupRepository _popupRepository;
        private PostsAdsRepository _postsAdsRepository;
        private ProfileRepository _profileRepository;
        private PostRepository _postRepository;

        public PermissionRepository Permission_Rep
        {
            get => GetOrCreate(ref _permissionRepository, () => new PermissionRepository(_configuration));
        }
        public PhanViecRepository PhanViec_Rep
        {
            get => GetOrCreate(ref _phanViecRepository, () => new PhanViecRepository());
        }
        public PhieuBaoLanhRepository PhieuBaoLanh_Rep
        {
            get => GetOrCreate(ref _phieuBaoLanhRepository, () => new PhieuBaoLanhRepository());
        }
        public PopupRepository Popup_Rep
        {
            get => GetOrCreate(ref _popupRepository, () => new PopupRepository());
        }
        public PostsAdsRepository PostsAds_Rep
        {
            get => GetOrCreate(ref _postsAdsRepository, () => new PostsAdsRepository(_configuration));
        }
        public ProfileRepository Profile_Rep
        {
            get => GetOrCreate(ref _profileRepository, () => new ProfileRepository(_configuration));
        }
        public PostRepository Post_Rep
        {
            get => GetOrCreate(ref _postRepository, () => new PostRepository(_configuration));
        }
        #endregion

        #region KEY_WORD: Q
        private QRRepository _qrRepository;

        public QRRepository QR_Rep
        {
            get => GetOrCreate(ref _qrRepository, () => new QRRepository(_configuration));
        }
        #endregion

        #region KEY_WORD: R
        ReportPaymentChannelRepository _reportPaymentChanneRepository;

        public ReportPaymentChannelRepository ReportPaymentChannel_Rep
        {
            get => GetOrCreate(ref _reportPaymentChanneRepository, () => new ReportPaymentChannelRepository());
        }
        #endregion

        #region KEY_WORD: S
        private SanPhamDichVuRepository _sanPhamDichVuRepository;
        private SupportRequestRepository _supportRequestRepository;
        private SinhnhatdailyRepository _sinhNhatRepository;
        public SanPhamDichVuRepository SPDV_Rep
        {
            get => GetOrCreate(ref _sanPhamDichVuRepository, () => new SanPhamDichVuRepository());
        }
        public SupportRequestRepository SupportRequest_Rep
        {
            get => GetOrCreate(ref _supportRequestRepository, () => new SupportRequestRepository());
        }

        public SinhnhatdailyRepository SinhNhat_Rep
        {
            get => GetOrCreate(ref _sinhNhatRepository, () => new SinhnhatdailyRepository(_sinhnhatDbContext,_configuration));
        }
        #endregion

        #region KEY_WORD: T
        private ThongBaoDaiLyRepository _thongBaoDaiLyRepository;
        private ThongBaoRepository _thongBaoRepository;
        private ThongTinDaiLyRepository _thongTinDaiLyRepository;
        private ThongTinHDRepository _thongTinHDRepository;
        private TourHotRepository _tourHotRepository;
        private TourLocationRepository _tourLocationRepository;

        public ThongBaoDaiLyRepository ThongBaoDaiLy_Rep
        {
            get => GetOrCreate(ref _thongBaoDaiLyRepository, () => new ThongBaoDaiLyRepository(_configuration));
        }
        public ThongBaoRepository ThongBao_Rep
        {
            get => GetOrCreate(ref _thongBaoRepository, () => new ThongBaoRepository());
        }
        public ThongTinDaiLyRepository ThongTinDaiLy_Rep
        {
            get => GetOrCreate(ref _thongTinDaiLyRepository, () => new ThongTinDaiLyRepository(_configuration));
        }
        public ThongTinHDRepository ThongTinHD_Rep
        {
            get => GetOrCreate(ref _thongTinHDRepository, () => new ThongTinHDRepository());
        }
        public TourHotRepository TourHot_Rep
        {
            get => GetOrCreate(ref _tourHotRepository, () => new TourHotRepository(_configuration));
        }
        public TourLocationRepository TourLocation_Rep
        {
            get => GetOrCreate(ref _tourLocationRepository, () => new TourLocationRepository(_configuration));
        }
        #endregion

        #region KEY_WORD: V
        private VeHoanRepository _veHoanRepository;
        private VeSotRepository _veSotRepository;
        private VisaRepository _visaRepository;
        private VoucherRepository _voucherRepository;

        public VeHoanRepository VeHoan_Rep
        {
            get => GetOrCreate(ref _veHoanRepository, () => new VeHoanRepository(_configuration));
        }
        public VeSotRepository VeSot_Rep
        {
            get => GetOrCreate(ref _veSotRepository, () => new VeSotRepository(_configuration));
        }
        public VisaRepository Visa_Rep
        {
            get => GetOrCreate(ref _visaRepository, () => new VisaRepository(_configuration));
        }
        public VoucherRepository Voucher_Rep
        {
            get => GetOrCreate(ref _voucherRepository, () => new VoucherRepository(_configuration));
        }
        #endregion

       
    }
}
