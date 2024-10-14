using Manager.DataAccess.Repository;

namespace Manager.DataAccess.IRepository.IUnitOfWork_Repository
{
    public interface IUnitOfWork_Repository
    {
        #region KEY_WORD: A
        ArticleRepository Article_Rep { get; }
        BangTinRepository BangTin_Rep { get; }
        #endregion

        #region KEY_WORD: B
        BankRepository Bank_Rep { get; }
        BankStatementRepository BankStatement_Rep{ get; }
        BienDongSoDuRepository BienDongSoDu_Rep { get; }
        BookerClubRepository BookerClub_Rep { get; }
        BookingRepository Booking_Rep { get; }
        #endregion

        #region KEY_WORD: C
        ChartRepository Chart_Rep { get; }
        ChuongTrinhKhuyenMaiRepository CTKM_Rep { get; }
        ChuongTrinhXoSoRepository CTXS_Rep { get; }
        ConfigBalanceAirlinesRepository ConfigBalanceAirlines_Rep{ get; }
        ConfigPhiXuatRepository ConfigPhiXuat_Rep{ get; }
        CongNoQuaHanRepository CongNoQuaHan_Rep { get; }
        CongNoRepository CongNo_Rep { get; }
        #endregion

        #region KEY_WORD: D 
        DaiLyRepository DaiLy_Rep { get; }
        DecentralizationRepository Decentralization_Rep { get; }
        DinhDanhBaoLuuRepository DinhDanhBaoLuu_Rep { get; }
        DoanhSoHangRepository DoanhSoHang_Rep { get; }
        DulichRepository Dulich_Rep { get; }
        #endregion

        #region KEY_WORD: E 
        EVMailRepository EVMail_Rep { get; }
        #endregion

        #region KEY_WORD: F 
        FareRulesRepository FareRules_Rep { get; }
        FlagRepository Flag_Rep { get; }
        FlightGroupRepository FlightGroup_Rep { get; }
        FlightRepository Flight_Rep { get; }
        ForumRepository Forum_Rep { get; }
        #endregion

        #region KEY_WORD: G 
        GatewayRepository Gateway_Rep { get; }
        GuiMailDaiLyRepository GuiMail_DaiLy_Rep { get; }
        #endregion

        #region KEY_WORD: H
        HangRepository Hang_Rep { get; }
        HotelRepository Hotel_Rep{ get; }
        #endregion

        #region KEY_WORD: I
        ImportDoanhSoRepository ImportDoanhSo_Rep { get; }
        IPRepository IP_Rep { get; }
        #endregion

        #region KEY_WORD: K
        KhachHangRepository KhachHang_Rep { get; }
        KhoaCodeDaiLyRepository KhoaCodeDaiLy_Rep { get; }
        KhoaHoaDonRepository KhoaHoaDon_Rep {  get; } 
        KyNiemRepository KyNiem_Rep { get; }
        #endregion

        #region KEY_WORD: L
        LichLamViecRepository LichLamViec_Rep { get; }
        LocationRepository Location_Rep { get; }
        Log_SendSMSRepository Log_SendSMS_Rep { get; }
        LotusmileRepository Lotusmile_Rep { get; }
        LoginRepository Login_Rep { get; }
        LandingPageRepository LandingPage_Rep { get; }
        #endregion

        #region KEY_WORD: M
        MemberRepository Member_Rep { get; }
        #endregion

        #region KEY_WORD: N
        NotifyRepository Notify_Rep { get; }
        #endregion

        #region KEY_WORD: P
        PermissionRepository Permission_Rep { get; }
        PhanViecRepository PhanViec_Rep { get; }
        PhieuBaoLanhRepository PhieuBaoLanh_Rep { get; }
        PopupRepository Popup_Rep { get; }
        PostsAdsRepository PostsAds_Rep { get; }
        ProfileRepository Profile_Rep { get; }
        PostRepository Post_Rep { get; }

        #endregion

        #region KEY_WORD: Q
        QRRepository QR_Rep { get; }
        #endregion

        #region KEY_WORD: R
        ReportPaymentChannelRepository ReportPaymentChannel_Rep { get; }
        #endregion

        #region KEY_WORD: S
        SanPhamDichVuRepository SPDV_Rep { get; }
        SupportRequestRepository SupportRequest_Rep { get; }
        #endregion

        #region KEY_WORD: T
        ThongBaoDaiLyRepository ThongBaoDaiLy_Rep { get; }
        ThongBaoRepository ThongBao_Rep { get; }
        ThongTinDaiLyRepository ThongTinDaiLy_Rep { get; }
        ThongTinHDRepository ThongTinHD_Rep { get; }
        TourHotRepository TourHot_Rep { get; }
        TourLocationRepository TourLocation_Rep { get; }
        #endregion

        #region KEY_WORD: V
        VeHoanRepository VeHoan_Rep { get; }
        VeSotRepository VeSot_Rep { get; }
        VisaRepository Visa_Rep { get; }
        VoucherRepository Voucher_Rep { get; }
        #endregion

    }
}
