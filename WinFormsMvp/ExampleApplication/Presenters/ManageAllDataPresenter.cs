﻿using System;
using System.Linq;
using ExampleApplication.Ioc;
using ExampleApplication.Models;
using ExampleApplication.Services;
using ExampleApplication.Views;
using WinFormsMvp;

namespace ExampleApplication.Presenters
{
    public class ManageAllDataPresenter : Presenter<IAllDataView>
    {
        private readonly ITimeTrackerService timeTrackerService;


        public ManageAllDataPresenter(IAllDataView view) : base(view)
        {
            timeTrackerService = ServiceLocator.Resolve<ITimeTrackerService>();
            View.ProjectDeleteSelected += new EventHandler(View_ProjectDeleteSelected);
            View.Load += new EventHandler(View_Load);
            View.ProjectHasBeenSelected += new EventHandler(View_ProjectHasBeenSelected);
            View.ProjectVisibilityToggled += new EventHandler(View_ProjectVisibilityToggled);
            View.TaskHasBeenSelected += new EventHandler(View_TaskHasBeenSelected);
            View.TaskDeleteSelected += new EventHandler(View_TaskDeleteSelected);
            View.TaskVisibilityToggled += new EventHandler(View_TaskVisibilityToggled);
            View.WorkItemDeleteSelected += new EventHandler<Custom.SelectedWorkItemEventArgs>(View_WorkItemDeleteSelected);
        }

        void View_WorkItemDeleteSelected(object sender, Custom.SelectedWorkItemEventArgs e)
        {
            timeTrackerService.DeleteWorkItem(e.SelectedWorkItem);
            View.PopulateWorkItemsByTaskId(timeTrackerService.GetWorkItemsOfTask((int)View.Model.SelectedTask.Id).ToList());
        }

        void View_TaskVisibilityToggled(object sender, EventArgs e)
        {
            timeTrackerService.UpdateTask(View.Model.SelectedTask);
            View.PopulateTasksByProjectId(timeTrackerService.GetTasksOfProject(View.Model.SelectedProject.Id).ToList());
        }

        void View_TaskDeleteSelected(object sender, EventArgs e)
        {
            timeTrackerService.DeleteTask(View.Model.SelectedTask);
            View.PopulateTasksByProjectId(timeTrackerService.GetTasksOfProject((int)View.Model.SelectedProject.Id).ToList());
        }

        void View_ProjectVisibilityToggled(object sender, EventArgs e)
        {
            timeTrackerService.UpdateProject(View.Model.SelectedProject);
            View.PopulateProjects(timeTrackerService.GetListOfProjects().ToList());
        }

        void View_ProjectDeleteSelected(object sender, EventArgs e)
        {
            timeTrackerService.DeleteProject(View.Model.SelectedProject);
            View.PopulateProjects(timeTrackerService.GetListOfVisibleProjects().ToList());
        }

        void View_TaskHasBeenSelected(object sender, EventArgs e)
        {
            View.PopulateWorkItemsByTaskId(timeTrackerService.GetWorkItemsOfTask((int)View.Model.SelectedTask.Id).ToList());
        }

        void View_ProjectHasBeenSelected(object sender, EventArgs e)
        {
            View.PopulateTasksByProjectId(timeTrackerService.GetTasksOfProject((int)View.Model.SelectedProject.Id).ToList());
        }

        void View_Load(object sender, EventArgs e)
        {
            View.Model = new ViewAllWorkModel
                             {
                                 Projects = timeTrackerService.GetListOfProjects().ToList()
                             };
        }

    }
}
