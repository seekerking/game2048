namespace Models.ViewModels.RpsModel
{
   public class RpsBase
   {
       public bool Success { get; set; } = false;

       public string ErrorMessage { get; set; } = string.Empty;

       public int Code { get; set; } = 0;
   }
}
