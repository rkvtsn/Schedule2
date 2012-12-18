using System;
using System.Web.Mvc;
using System.Web.Security;
using Mvc_Schedule.Models.DataModels.ModelViews;

namespace Mvc_Schedule.Controllers
{
    public class AuthController : Controller
    {
		//
		// GET: /Account/LogOn

		public ActionResult LogOn()
		{
			if (Request.IsAuthenticated)
				return RedirectToAction("Index", "Default");
			return View();
		}

		//
		// POST: /Account/LogOn

		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult LogOn(LogOnModel model, string returnUrl)
		{
			if (ModelState.IsValid)
			{
				if (Membership.ValidateUser(model.UserName, model.Password))
				{
					FormsAuthentication.SetAuthCookie(model.UserName, model.RememberMe);
					if (Url.IsLocalUrl(returnUrl) && returnUrl.Length > 1 && returnUrl.StartsWith("/")
						&& !returnUrl.StartsWith("//") && !returnUrl.StartsWith("/\\"))
					{
						return Redirect(returnUrl);
					}
					else
					{
						return RedirectToAction("Index", "Default");
					}
				}
				else
				{
					ModelState.AddModelError("", "Имя пользователя или пароль указаны неверно.");
				}
			}

			// Появление этого сообщения означает наличие ошибки; повторное отображение формы
			return View(model);
		}

		public ActionResult LogOff()
		{
			FormsAuthentication.SignOut();

			return RedirectToAction("Index", "Default");
		}

		public ActionResult Register()
		{
			return View();
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Register(RegisterModel model)
		{
			if (ModelState.IsValid)
			{
				// Попытка зарегистрировать пользователя
				MembershipCreateStatus createStatus;
				Membership.CreateUser(model.UserName, model.Password, model.Email, null, null, true, null, out createStatus);

				if (createStatus == MembershipCreateStatus.Success)
				{
					FormsAuthentication.SetAuthCookie(model.UserName, false /* createPersistentCookie */);
					return RedirectToAction("Index", "Default");
				}
				else
				{
					ModelState.AddModelError("", ErrorCodeToString(createStatus));
				}
			}

			// Появление этого сообщения означает наличие ошибки; повторное отображение формы
			return View(model);
		}

		[Authorize]
		public ActionResult ChangePassword()
		{
			return View();
		}

		[Authorize]
		[HttpPost]
		public ActionResult ChangePassword(ChangePasswordModel model)
		{
			if (ModelState.IsValid)
			{

				// При некоторых сценариях сбоя операция смены пароля ChangePassword вызывает исключение,
				// а не возвращает значение false (ложь).
				bool changePasswordSucceeded;
				try
				{
					MembershipUser currentUser = Membership.GetUser(User.Identity.Name, true /* userIsOnline */);
					changePasswordSucceeded = currentUser.ChangePassword(model.OldPassword, model.NewPassword);
				}
				catch (Exception)
				{
					changePasswordSucceeded = false;
				}

				if (changePasswordSucceeded)
				{
					return RedirectToAction("ChangePasswordSuccess");
				}
				else
				{
					ModelState.AddModelError("", "Неправильный текущий пароль или недопустимый новый пароль.");
				}
			}

			// Появление этого сообщения означает наличие ошибки; повторное отображение формы
			return View(model);
		}

		public ActionResult ChangePasswordSuccess()
		{
			return View();
		}

		#region Status Codes
		private static string ErrorCodeToString(MembershipCreateStatus createStatus)
		{
			// Полный список кодов состояния см. по адресу http://go.microsoft.com/fwlink/?LinkID=177550
			//.
			switch (createStatus)
			{
				case MembershipCreateStatus.DuplicateUserName:
					return "Имя пользователя уже существует. Введите другое имя пользователя.";

				case MembershipCreateStatus.DuplicateEmail:
					return "Имя пользователя для данного адреса электронной почты уже существует. Введите другой адрес электронной почты.";

				case MembershipCreateStatus.InvalidPassword:
					return "Указан недопустимый пароль. Введите допустимое значение пароля.";

				case MembershipCreateStatus.InvalidEmail:
					return "Указан недопустимый адрес электронной почты. Проверьте значение и повторите попытку.";

				case MembershipCreateStatus.InvalidAnswer:
					return "Указан недопустимый ответ на вопрос для восстановления пароля. Проверьте значение и повторите попытку.";

				case MembershipCreateStatus.InvalidQuestion:
					return "Указан недопустимый вопрос для восстановления пароля. Проверьте значение и повторите попытку.";

				case MembershipCreateStatus.InvalidUserName:
					return "Указано недопустимое имя пользователя. Проверьте значение и повторите попытку.";

				case MembershipCreateStatus.ProviderError:
					return "Поставщик проверки подлинности вернул ошибку. Проверьте введенное значение и повторите попытку. Если проблему устранить не удастся, обратитесь к системному администратору.";

				case MembershipCreateStatus.UserRejected:
					return "Запрос создания пользователя был отменен. Проверьте введенное значение и повторите попытку. Если проблему устранить не удастся, обратитесь к системному администратору.";

				default:
					return "Произошла неизвестная ошибка. Проверьте введенное значение и повторите попытку. Если проблему устранить не удастся, обратитесь к системному администратору.";
			}
		}
		#endregion
    }
}
