﻿using EmployeeMangement.Models;
using EmployeeMangement.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeMangement.controllers
{

  //  [Authorize(Roles = "admin")]
    public class AdministratorController : Controller
    {
        private readonly ILogger<AdministratorController> logger;

        public RoleManager<IdentityRole> RoleMangerService { get; }
        public UserManager<ApplicationUser> UserManager { get; }
        public AdministratorController(RoleManager<IdentityRole> roleMangerService, UserManager<ApplicationUser> userManager,
                                        ILogger<AdministratorController> logger)
        {
            RoleMangerService = roleMangerService;
            UserManager = userManager;
            this.logger = logger;
        }

        [HttpGet]
        public IActionResult CreateRole()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CreateRole(CreateRoleViewModel model)
        {
            if (ModelState.IsValid)
            {
                IdentityRole identityRole = new IdentityRole
                {
                    Name = model.RoleName
                };

                var result = await RoleMangerService.CreateAsync(identityRole);
                if (result.Succeeded)
                {
                    return RedirectToAction("ListRole", "Administrator");
                }
                foreach (IdentityError error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }

            return View(model);
        }
        [HttpGet]
        public IActionResult ListRole()
        {
            var roles = RoleMangerService.Roles;
            return View(roles);
        }


        [HttpGet]
        public async Task<IActionResult> EditRole(string id)
        {

            var role = await RoleMangerService.FindByIdAsync(id);
            if (role != null)
            {
                var model = new EditRoleViewModel
                {
                    Id = role.Id,
                    RoleName = role.Name,
                };


                foreach (var user in await UserManager.GetUsersInRoleAsync(role.Name))
                {
                    model.Users.Add(user.UserName);
                }

                return View(model);
            }

            ViewBag.ErrorMassage = $"role with id: {id} cannot be found!";
            return View("NotFound");
        }

        [HttpPost]
        public async Task<IActionResult> EditRole(EditRoleViewModel model)
        {
            var role = await RoleMangerService.FindByIdAsync(model.Id);
            if (role != null)
            {
                role.Name = model.RoleName;
                var result = await RoleMangerService.UpdateAsync(role);
                if (result.Succeeded)
                {
                    return RedirectToAction("ListRole");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                //in case there were errors, return to edit
                return View(model);

            }

            ViewBag.ErrorMassage = $"role with id: {model.Id} cannot be found!";
            return View("NotFound");
        }

        [HttpGet]
        public async Task<IActionResult> EditUsersInRole(string roleId)
        {
            ViewBag.roleId = roleId;
            var role = RoleMangerService.FindByIdAsync(roleId);
            if (role == null)
            {
                ViewBag.ErrorMessage = $"The role with id : {roleId} cannot be found";
                return View("NotFound");
            }
            /*
            var usersInRole = await UserManager.GetUsersInRoleAsync(role.Name);
            if (usersInRole == null)
            {
                ViewBag.ErrorMessage = $"No users for this role id {roleId}.";
            }
            */

            var model = new List<UserRoleViewModel>();
            var allUsers = UserManager.Users;
            var UserInThisRole = await UserManager.GetUsersInRoleAsync(role.Result.Name);
            foreach (var user in allUsers)
            {
                var userRoleViewModel = new UserRoleViewModel
                {
                    UserId = user.Id,
                    UserName = user.UserName,
                    IsSelected = false

                };

                if (UserInThisRole.Any(x => x.Id == userRoleViewModel.UserId))
                    userRoleViewModel.IsSelected = true;

                model.Add(userRoleViewModel);
            }
            return View(model);

        }

        // part 81

        [HttpPost]
        public async Task<IActionResult> EditUsersInRole(List<UserRoleViewModel> model, string roleId)
        {
            var role = RoleMangerService.FindByIdAsync(roleId);
            if (role == null)
            {
                ViewBag.ErrorMessage = $"The role with id : {roleId} cannot be found";
                return View("NotFound");
            }

            for (int i = 0; i < model.Count; i++)
            {
                var user = await UserManager.FindByIdAsync(model[i].UserId);
                IdentityResult result = null;
                var isInRole = await UserManager.IsInRoleAsync(user, role.Result.Name);
                if (model[i].IsSelected && !(isInRole))
                {
                    result = await UserManager.AddToRoleAsync(user, role.Result.Name);
                }
                else if (!model[i].IsSelected && isInRole)
                {
                    result = await UserManager.RemoveFromRoleAsync(user, role.Result.Name);
                }
                else
                {
                    continue;
                }

                if (result.Succeeded)
                {
                    if (i < model.Count - 1)
                    {
                        continue;
                    }
                    else
                    {
                        return RedirectToAction("EditRole", new { id = roleId });
                    }
                }
            }
            return RedirectToAction("EditRole", new { id = roleId });
        }

        [HttpGet]
        public IActionResult ListUser()
        {
            var users = UserManager.Users;
            return View(users);
        }

        [HttpGet]
        public async Task<IActionResult> EditUser(string id)
        {
            var user = await UserManager.FindByIdAsync(id);
            if (user == null)
            {
                ViewBag.ErrorMessage = $"User with id: {id} cannot be found";
                return View("NotFound");
            }
            var userRoles = await UserManager.GetRolesAsync(user);
            var userClaims = await UserManager.GetClaimsAsync(user);
            var model = new EditUserViewModel
            {
                Id = id,
                City = user.city,
                Email = user.Email,
                UserName = user.UserName,
                Roles = userRoles,
                Claims = userClaims.Select(c => c.Value).ToList()
            };

            return View(model);
        }


        [HttpPost]
        public async Task<IActionResult> EditUser(EditUserViewModel model)
        {
            var user = await UserManager.FindByIdAsync(model.Id);
            if (user == null)
            {
                ViewBag.ErrorMessage = $"User with id: {model.Id} cannot be found";
                return View("NotFound");
            }
            else
            {
                user.Email = model.Email;
                user.UserName = model.UserName;
                user.city = model.City;

                var result = await UserManager.UpdateAsync(user);
                if (result.Succeeded)
                {
                    return RedirectToAction("ListUser");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return View(model);
            }
        }
        [HttpPost]
        public async Task<IActionResult> DeleteUser(string id)
        {
            var user = await UserManager.FindByIdAsync(id);
            if (user == null)
            {
                ViewBag.ErrorMessage = $"User with id: {id} cannot be found";
                return View("NotFound");
            }
            else
            {
                var result = await UserManager.DeleteAsync(user);
                if (result.Succeeded)
                {
                    return RedirectToAction("ListUser");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return View("LisrUser");
            }
        }

        [HttpPost]
        public async Task<IActionResult> DeleteRole(string roleId)
        {
            var role = await RoleMangerService.FindByIdAsync(roleId);
            if (role == null)
            {
                ViewBag.ErrorMessage = $"Role with id: {roleId} cannot be found";
                return View("NotFound");
            }
            else
            {
                try
                {
                    var result = await RoleMangerService.DeleteAsync(role);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("ListRole");
                    }
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                    return View("LisrRole");
                }

                catch (DbUpdateException e)
                {
                    logger.LogError($"Error deleting role {e}");
                    ViewBag.ErrorTitle = $"{role.Name} role is in use";
                    ViewBag.ErrorMessage = $"{role.Name} role cannot be deleted as there are users in this role";
                    return View("Error");
                }
            }
        }
        [HttpGet]
        public async Task<IActionResult> ManageUserRoles(string userId)
        {
            ViewBag.userID = userId;

            var user = await UserManager.FindByIdAsync(userId);
            if (user ==null)
            {
                ViewBag.ErrorMessage = $"User with id {userId} cannot be found";
                return View("NotFound");
            }

            List<UserRolesViewModel> model = new List<UserRolesViewModel>();
            foreach (var role in await RoleMangerService.Roles.ToListAsync())
            {
                UserRolesViewModel userRolesViewModel = new UserRolesViewModel
                {
                    RoleId = role.Id,
                    RoleName = role.Name,
                    IsSelected = false
                };
                if (await UserManager.IsInRoleAsync(user, role.Name))
                {
                    userRolesViewModel.IsSelected = true;
                }

                model.Add(userRolesViewModel);
            }

            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> ManageUserRoles(List<UserRolesViewModel> userRolesViewModel, string id)
        {
            var user = await UserManager.FindByIdAsync(id);
            if (user == null)
            {
                ViewBag.ErrorMessage = $"User with id {id} cannot be found from post";
                return View("NotFound");
            }
            var roles = await UserManager.GetRolesAsync(user);
            var result = await UserManager.RemoveFromRolesAsync(user, roles);
            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "cannot remove user existing roles");
                return View(userRolesViewModel);
            }
            result = await UserManager.AddToRolesAsync(user, userRolesViewModel.Where(x => x.IsSelected).Select(y => y.RoleName));
            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "cannot add selected roles to user");
                return View(userRolesViewModel);
            }

            return RedirectToAction("EditUser", new { id = id });
        }
    }
}


