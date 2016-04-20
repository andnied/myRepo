﻿using System;
using System.Configuration;
using System.Linq;
using ExampleApplication.DataAccess;

namespace ExampleApplication.Services
{
    public class TimeTrackerService : ITimeTrackerService
    {
        private UnitOfWork unitOfWork;

        public TimeTrackerService()
        {
            unitOfWork =
                new UnitOfWork(
                    new TimeTrackerEntities(
                        ConfigurationManager.ConnectionStrings["TimeTrackerEntities"].ConnectionString));
        }

        public void CreateNewProject(string name, bool visibility, string description = null)
        {
            unitOfWork.CreateNewProject(name, visibility, description);
        }

        public void CreateNewTask(string name, bool visibility, Project project, decimal estimate,
                                  string description = null)
        {
            unitOfWork.CreateNewTask(name, visibility, project, estimate, description);
        }

        public void CreateNewWorkItem(Task task, double duration, DateTime dateOfWork, string description = null)
        {
            unitOfWork.CreateNewWorkItem(task, duration, dateOfWork, description);
        }

        public void DeleteProject(Project project)
        {
            unitOfWork.DeleteProject(project);
        }

        public void DeleteTask(Task task)
        {
            unitOfWork.DeleteTask(task);
        }

        public void DeleteWorkItem(Work work)
        {
            unitOfWork.DeleteWorkItem(work);
        }

        public IQueryable<Project> GetListOfProjects()
        {
            return unitOfWork.GetAllProjects();
        }


        public IQueryable<Project> GetListOfVisibleProjects()
        {
            return unitOfWork.GetAllProjects().Where(p => p.Visible);
        }

        public IQueryable<Task> GetTasksOfProject(int taskId)
        {
            return unitOfWork.GetTasksOfProject(taskId);
        }

        public IQueryable<Task> GetVisibleTasksOfProject(int projectId)
        {
            return unitOfWork.GetTasksOfProject(projectId).Where(p => p.Visible);
        }

        public IQueryable<Work> GetWorkItemsOfTask(int taskId)
        {
            return unitOfWork.GetWorkItemsOfTask(taskId);
        }

        public void UpdateProject(Project project)
        {
            this.unitOfWork.UpdateProject(project);
        }

        public void UpdateTask(Task task)
        {
            this.unitOfWork.UpdateTask(task);
        }
    }
}
