﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using QuanLyNhanSu.Commons;
using QuanLyNhanSu.Helpers;
using QuanLyNhanSu.Interfaces;
using QuanLyNhanSu.Models;
using QuanLyNhanSu.ViewModels.Account;
using QuanLyNhanSu.ViewModels.Role;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace QuanLyNhanSu.Controllers
{
    public class AccountController : BaseController
    {
        private readonly IAccountService _accountService;
        private readonly IRoleService _roleService;
        private readonly IRoleEmployeeService _roleEmployeeService;
        private readonly IEmployeeService _employeeService;
        public AccountController(IAccountService accountService, IRoleService roleService, IRoleEmployeeService roleEmployeeService, IEmployeeService employeeService)
        {
            _accountService = accountService;
            _roleService = roleService;
            _roleEmployeeService = roleEmployeeService;
            _employeeService = employeeService;
        }

        public async Task<IActionResult> Index(string search, int? pageNumber)
        {
            var accounts = _accountService.GetAllAccounts(search);
            int pageSize = 1;
            return View(await PaginatedList<Login>.CreateAsync(accounts.AsQueryable(), pageNumber ?? 1, pageSize));
        }

        [HttpGet]
        public async Task<IActionResult> AddRoleToAccount(int id)
        {
            var roles = _roleService.GetAllRoles().ToList();
            var roleEmployees = await _roleEmployeeService.GetRoleEmployeeByIdAccount(id);
            AddRoleToAccountViewModel model = new AddRoleToAccountViewModel()
            {
                Id = id,
                drpRoles = roles.Select(x => new SelectListItem { Text = x.MaQuyen, Value = x.MaQuyen }).ToList(),
                Roles = roleEmployees.Select(x=>x.MaQuyen).ToArray(),
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AddRoleToAccount(AddRoleToAccountViewModel model)
        {
            ValidateResult resultRoles = model.Roles.Length.ToString().ValidateAccount("Roles", "") == null ? null : model.Roles.Length.ToString().ValidateAccount("Roles", "");
            if (resultRoles._isNull == false )
            {
                ViewBag.resultPassword = resultRoles;

                return View();
            }
            var isSuccess = await _accountService.AddRoleToAccount(model);
            if (isSuccess == 1)
            {
                return RedirectToAction("Index", "Account");
            }
            else
            {
                return View();
            }
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(AddAccountViewModel model)
        {
            ValidateResult resultUsername = model.Username.ValidateAccount("username", "") == null ? null : model.Username.ValidateAccount("username", "");
            ValidateResult resultPassword = model.Password.ValidateAccount("password", "") == null ? null : model.Password.ValidateAccount("password", "");
            ValidateResult resultComfirmPass = model.Password.ValidateAccount("", model.ConfirmPassword) == null ? null : model.Password.ValidateAccount("", model.ConfirmPassword);
            ValidateResult resultEmail = model.Email.ValidateAccount("email", "") == null ? null : model.Email.ValidateAccount("email", "");
            if(resultUsername._isNull == false || resultPassword._isNull == false || resultComfirmPass._isNull == false || resultEmail._isNull == false)
            {
                ViewBag.resultUsername = resultUsername;
                ViewBag.resultPassword = resultPassword;
                ViewBag.resultComfirmPass = resultComfirmPass;
                ViewBag.resultEmail = resultEmail;
                return View();
            }
            model.CreatedAt = System.DateTime.Now;
            var isSuccess = await _accountService.AddAccount(model);
            if(isSuccess == 1)
            {
                return RedirectToAction("Index", "Account");
            }
            else
            {
                return View();
            }
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            var account = await _accountService.GetAccountById(Convert.ToInt32(id));
            return View(account);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditAccountViewModel model)
        {
            ValidateResult resultUsername = model.Username.ValidateAccount("username", "") == null ? null : model.Username.ValidateAccount("username", "");
            ValidateResult resultPassword = model.Password.ValidateAccount("password", "") == null ? null : model.Password.ValidateAccount("password", "");
            ValidateResult resultComfirmPass = model.Password.ValidateAccount("", model.ConfirmPassword) == null ? null : model.Password.ValidateAccount("", model.ConfirmPassword);
            ValidateResult resultEmail = model.Email.ValidateAccount("email", "") == null ? null : model.Email.ValidateAccount("email", "");
            if (resultUsername._isNull == false || resultPassword._isNull == false || resultComfirmPass._isNull == false || resultEmail._isNull == false)
            {
                ViewBag.resultUsername = resultUsername;
                ViewBag.resultPassword = resultPassword;
                ViewBag.resultComfirmPass = resultComfirmPass;
                ViewBag.resultEmail = resultEmail;
                return View();
            }
            model.UpdatedAt = System.DateTime.Now;

            var isSuccess = await _accountService.EditAccount(model);
            if (isSuccess == 1)
            {
                return RedirectToAction("Index", "Account");
            }
            else
            {
                return View();
            }
        }

        [HttpGet]
        public async Task<IActionResult> Detail(string id)
        {
            var emp = await _employeeService.GetEmployeeById(id);
            var account = await _accountService.GetAccountById(emp.Idlogin.Value);
            return View(account);
        }

        [HttpPost]
        public async Task<IActionResult> Detail(EditAccountViewModel model)
        {
            ValidateResult resultUsername = model.Username.ValidateAccount("username", "") == null ? null : model.Username.ValidateAccount("username", "");
            ValidateResult resultPassword = model.Password.ValidateAccount("password", "") == null ? null : model.Password.ValidateAccount("password", "");
            ValidateResult resultComfirmPass = model.Password.ValidateAccount("", model.ConfirmPassword) == null ? null : model.Password.ValidateAccount("", model.ConfirmPassword);
            ValidateResult resultEmail = model.Email.ValidateAccount("email", "") == null ? null : model.Email.ValidateAccount("email", "");
            if (resultUsername._isNull == false || resultPassword._isNull == false || resultComfirmPass._isNull == false || resultEmail._isNull == false)
            {
                ViewBag.resultUsername = resultUsername;
                ViewBag.resultPassword = resultPassword;
                ViewBag.resultComfirmPass = resultComfirmPass;
                ViewBag.resultEmail = resultEmail;
                return View();
            }
            model.UpdatedAt = System.DateTime.Now;

            var isSuccess = await _accountService.EditAccount(model);
            if (isSuccess == 1)
            {
                return RedirectToAction("Index", "Account");
            }
            else
            {
                return View();
            }
        }

        public async Task<JsonResult> Delete(int id)
        {
            var isSuccess = await _accountService.DeleteAccount(id);
            if (isSuccess == 1)
            {
                return Json(new
                {
                    status = true
                });
            }
            else
            {
                return Json(new
                {
                    status = false
                });
            }
        }
    }
}
