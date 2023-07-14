using JYD.DataContext.DbModel;

namespace JYD.DataContext.Repository
{
    public interface IProcedureRepository
    {
        Task<Agency_Ref?> GetAgency_Ref(string agency_code);
        Task<UserDB?> GetDBs(string user_id, string password_hash, string temp_password);
        Task<UserAuditDB?> GetUserAudit(string User_id);
        Task<int> UpdateUserPassword(string user_id, string password_hash);
        Task<Retrieve_UserdtlDB?> GetUserDetail(string User_id);
        Task<int> ForgotUserPassword(string user_id, string temp_password,string email_address);
        Task<int> UpdateSession(string user_id, string session_flag);
        Task<List<DealerDB?>> GetListOfDealer(string site_code);
        Task<List<ItemDB?>> GetListOfItem(string dealer_code);
        Task<List<AllocationDtlDB?>> GetAllocationDtlperNo(string control_no);
        Task<Validate?> GetValidate(string item_code, string range_fr, string range_to);
        Task<List<CheckDuplicate>?> GetCheckDuplicate(string item_code, string range_fr, string range_to);
        Task<Exploded?> GetExploded(string control_no, string ref_no, string site_code, string agency_code, string item_code, string range_fr, string range_to, string status_code, string user_id);
        Task<int> DeleteAllocationDtl(string control_no, int seq_no,string user_id);
        Task<NewAllocationDB?> ProcessNewAllocation(string control_no, string user_id);
        Task<int> DelAllocHDRDetails(string control_no, int seq_no);
        Task<int> CancellAllocation(string control_no);
        Task<int> RemoveTMAllocation(string user_id);
        Task<int> SaveNotification(string id, string userid, string agency_code, string refno, string messasge);
        Task<List<Notification>?> GetNotification(string userid);
        Task<Notification?> GetNotificationById(string Id);
        Task<int> ReadNotification(string id, string userid);        
        Task<List<Allocation_Inquiry>?> Allocation_Inquiries(string site_code,string agency_code,string ref_no,string item_code, string status, DateTime date_from, DateTime date_to);
        Task<List<Status_Ref>?> Status_Ref(string table_name);
        Task<List<ReportbyRefNo>?> ReportbyRefNo(string dealer, string refno);
        Task<int> ProcessAcceptance(string ref_no, string user_id);
        Task<List<ReportbyPlateNo>?> ReportbyPlateNos(string plate_no, string site_code, string agency_code);
        Task<allochdr?> allochdr(string ref_no);
        Task<int> UpdateAllocationDtl(string plate_no, string status_code, string engine_no, string chassis_no, DateTime date_reserved, string user_id);

        //Task<int> allex(string control_no, int seq_no, string plate_no, string user_id);
        Task<List<AllocationException>?> allex(string control_no, int seq_no, string plate_no, string user_id);
        Task<List<AllocationException>?> delallex(string control_no, int seq_no, string plate_no, string user_id);
        Task<List<AllocationException>?> GetAlloctionException(string control_no, int seq_no);
        Task<List<Validate_PlateEngChass>?> validate_PEC(string agency_code,string plate_no,string status_code,string engine_no,string chassis_no,DateTime date_reserved,string item_code, string old_values,string user_id);
        Task<List<GetPlateFIFO>?> plateFIFOs(string agency_code, string item_code);
        Task<List<PlateAllocationReport>?> PlateAllocationRep(string site_code, string agency_code, string ref_no, string item_code, string status, DateTime date_from, DateTime date_to);
        Task<List<PlateAllocationReportDtl>?> PlateAllocationRepDtl(string ref_no, string status);
        Task<List<RevertPlateAllocation>?> RevertPlateAlloc(string ref_no, string user_id);
        Task<int> DeletePlateAssignment(string plateno, string user_id);  

    }
}