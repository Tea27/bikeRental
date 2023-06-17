//using Microsoft.AspNetCore.Mvc.Filters;
////using Microsoft.AspNetCore.Mvc;
//using System.Web.Mvc;

//namespace bikeRental.Frontend.Middleware;
//public class ExceptionHandler : HandleErrorAttribute
//{
//    public override void OnException(ExceptionContext filterContext)
//    {
//        if (filterContext.ExceptionHandled || filterContext.HttpContext.IsCustomErrorEnabled)
//        {
//            return;
//        }
//        Exception e = filterContext.Exception;
//        filterContext.ExceptionHandled = true;
//        filterContext.Result = new ViewResult()
//        {
//            ViewName = "Error2"
//        };
//    }
//}
