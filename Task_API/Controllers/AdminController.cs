﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Task_API.DBContext;
using Task_API.Interfaces;
using Task_API.ManualClasses;
using Task_API.Model;
using Task_API.Services;

namespace Task_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        TaskDataBaseContext TaskDataBaseContext;
        private readonly IAdminRepository _adminRepository;
        private readonly ITasksRepository _tasksRepository;
        private readonly ILogger<AdminController> _logger;

        public AdminController(TaskDataBaseContext taskDataBaseContext, IAdminRepository adminRepository, ITasksRepository tasksRepository, ILogger<AdminController> logger)
        {
            TaskDataBaseContext = taskDataBaseContext;
            _adminRepository = adminRepository;
            _tasksRepository = tasksRepository;
            _logger = logger;
        }

        [HttpGet]
        [Route("GetUserWithUsername")]
        public async Task<ActionResult> GetUserWithUsername(string userName)
        {
            try
            {
                var searchedUser = _adminRepository.SearchingUserWithUName(userName);
                if (searchedUser != null)
                {
                    return StatusCode(200, searchedUser);
                }
                else
                {
                    return StatusCode(404, "User Not Found...");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(500, "Internal Server Error...");
            }
        }

        [HttpGet]
        [Route("GetAllTasks")]
        public async Task<ActionResult> GetAllTasks(bool isAccending)
        {
            try
            {
                var allTasks = await _adminRepository.GetAllTasks(isAccending);
                return StatusCode(200, allTasks);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(500, "Internal Server Error...");
            }
        }

        [HttpGet]
        [Route("GetAllTaskOfUser")]
        public async Task<ActionResult> GetAllTaskOfUser(string userName)
        {
            try
            {
                var taskList = await _adminRepository.GetAllTaskFromUsername(userName);
                if (taskList == null)
                {
                    return NotFound();
                }
                else
                {
                    return StatusCode(200, taskList);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(500, "Intrnal Server error...");
            }
        }

        [HttpGet]
        [Route("SearchingFromTitle")]
        public async Task<ActionResult> SearchingFromTitle(string title)
        {
            try
            {
                var task = await _adminRepository.SearchAnyTask(title);
                return StatusCode(200, task);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(500, "Internal Server Error...");
            }
        }

        [HttpPut]
        [Route("UpdatingTask")]
        public async Task<ActionResult<MUpdatingTask>> UpdatingTask(MUpdatingTask mUpdatingTask)
        {
            try
            {
                var updatingTask = await _adminRepository.UpdatingTask(mUpdatingTask);
                return StatusCode(202, "Task Updated Successfully...");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(500, "Internal Server Error...");
            }
        }


        [HttpPut]
        [Route("ReAssignTaskToUser")]
        public async Task<ActionResult> ReAssignTaskToUser(string titleToAssign, string newUser)
        {
            try
            {
                var reassingning = await _adminRepository.ReAssignTask(titleToAssign, newUser);
                return StatusCode(202, "Task Re Assigned Successfully...");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(500, "Internal Server Error...");
            }
        }

        [HttpDelete]
        [Route("DeleteTask")]
        public async Task<ActionResult> DeleteTask(string taskTitle)
        {
            try
            {
                var delTask = await _adminRepository.DeletingTask(taskTitle);
                return StatusCode(200, "Task Deleted Sucessfully...");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(500, "Internal Server Error...");
            }
        }

    }
}