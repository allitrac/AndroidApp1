﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace PScore
{
    public class PsCore
    {

        private string siteURL = "https://sharepointevo.sharepoint.com/sites/mobility", psRestUrl = "/_api/ProjectServer";
        public string rtFa { get; set; }
        public string FedAuth { get; set; }
        HttpClient client;
        HttpClientHandler handler;

        public PsCore(HttpClientHandler handler)
        {
            this.handler = handler;
        }

        //used for GetAsync
        public void setClient()
        {
            if (client != null)
                client = null;

            var mediaType = new MediaTypeWithQualityHeaderValue("application/json");
            mediaType.Parameters.Add(new NameValueHeaderValue("odata", "verbose"));

            client = new HttpClient(handler);
            client.DefaultRequestHeaders.Accept.Add(mediaType);

        }

        //used for PostAsync
        //method 0 = no additional headers, 1 = MERGE, 2 = PUT, 3 = DELETE
        public void setClient(string formDigest, int method)
        {
            if (client != null)
                client = null;

            var mediaType = new MediaTypeWithQualityHeaderValue("application/json");
            mediaType.Parameters.Add(new NameValueHeaderValue("odata", "verbose"));

            client = new HttpClient(handler);
            client.DefaultRequestHeaders.Accept.Add(mediaType);
            client.DefaultRequestHeaders.Add("X-RequestDigest", formDigest);
            if (method == 1)
                client.DefaultRequestHeaders.Add("X-HTTP-METHOD", "MERGE");

        }

        public async Task<String> GetFormDigest(String body)
        {

            String response = "";

            var contents = new StringContent(body, Encoding.UTF8, "application/json");

            try
            {
                var result = await client.PostAsync(siteURL + "/_api/contextinfo", contents);
                var postResult = result.EnsureSuccessStatusCode();
                if (postResult.IsSuccessStatusCode)
                    response = await postResult.Content.ReadAsStringAsync();

                return response;
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }

        public async Task<String> GetEPT()
        {

            try
            {
                var result = await client.GetStringAsync(siteURL + psRestUrl + "EnterpriseProjectTypes");
                return result;
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }

        public async Task<bool> CheckOut(string body, string projectGUID)
        {
            Boolean isSuccess = false;
            var contents = new StringContent(body, Encoding.UTF8, "application/json");

            try
            {
                var result = await client.PostAsync(siteURL + psRestUrl + "/Projects('" + projectGUID + "')/CheckOut()", contents);
                var postResult = result.EnsureSuccessStatusCode();
                if (postResult.IsSuccessStatusCode)
                    isSuccess = true;

                return isSuccess;
            }
            catch (Exception e)
            {
                return isSuccess;
            }

        }

        public async Task<bool> CheckIn(string body, string projectGUID)
        {
            Boolean isSuccess = false;
            var contents = new StringContent(body, Encoding.UTF8, "application/json");

            try
            {
                var result = await client.PostAsync(siteURL + psRestUrl + "/Projects('" + projectGUID + "')/Draft/CheckIn()", contents);
                var postResult = result.EnsureSuccessStatusCode();
                if (postResult.IsSuccessStatusCode)
                    isSuccess = true;

                return isSuccess;
            }
            catch (Exception e)
            {
                return isSuccess;
            }

        }

        public async Task<bool> Publish(string body, string projectGUID)
        {

            Boolean isSuccess = false;
            var contents = new StringContent(body, Encoding.UTF8, "application/json");

            try
            {
                var result = await client.PostAsync(siteURL + psRestUrl + "/Projects('" + projectGUID + "')/Draft/Publish(true)", contents);
                var postResult = result.EnsureSuccessStatusCode();
                if (postResult.IsSuccessStatusCode)
                    isSuccess = true;

                return isSuccess;
            }
            catch (Exception e)
            {
                return isSuccess;
            }
        }

        public async Task<String> GetProjects()
        {

            try
            {
                var result = await client.GetStringAsync(siteURL + psRestUrl + "/Projects");
                return result;
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }

        public async Task<String> GetProjectById(string projectGUID)
        {

            try
            {
                var result = await client.GetStringAsync(siteURL + psRestUrl + "/Projects(guid'" + projectGUID + "')");
                return result;
            }
            catch (Exception e)
            {
                return e.Message;
            }

        }

        public async Task<bool> AddProjects(String body)
        {
            Boolean isSuccess = false;
            var contents = new StringContent(body, Encoding.UTF8, "application/json");

            try
            {
                var result = await client.PostAsync(siteURL + psRestUrl + "/Projects/Add", contents);
                var postResult = result.EnsureSuccessStatusCode();
                if (postResult.IsSuccessStatusCode)
                    isSuccess = true;

                return isSuccess;
            }
            catch (Exception e)
            {
                return isSuccess;
            }

        }

        public async Task<bool> UpdateProject(string body, string projectGUID)
        {

            Boolean isSuccess = false;
            var contents = new StringContent(body, Encoding.UTF8, "application/json");

            try
            {
                var result = await client.PostAsync(siteURL + psRestUrl + "/Projects('" + projectGUID + "')/Draft/update()", contents);
                var postResult = result.EnsureSuccessStatusCode();
                if (postResult.IsSuccessStatusCode)
                    isSuccess = true;

                return isSuccess;
            }
            catch (Exception e)
            {
                return isSuccess;
            }

        }

        public async Task<bool> DeleteProject(string body, string projectGUID)
        {

            Boolean isSuccess = false;
            var contents = new StringContent(body, Encoding.UTF8, "application/json");

            try
            {
                var result = await client.PostAsync(siteURL + psRestUrl + "/Projects('" + projectGUID + "')/deleteObject()", contents);
                var postResult = result.EnsureSuccessStatusCode();
                if (postResult.IsSuccessStatusCode)
                    isSuccess = true;

                return isSuccess;
            }
            catch (Exception e)
            {
                return isSuccess;
            }

        }

        public async Task<String> GetTasks(string projectGUID)
        {

            try
            {
                var result = await client.GetStringAsync(siteURL + psRestUrl + "/Projects('" + projectGUID + "')/Tasks");
                return result;
            }
            catch (Exception e)
            {
                return e.Message;
            }

        }

        public async Task<bool> AddTask(string body, string projectGUID)
        {

            Boolean isSuccess = false;
            var contents = new StringContent(body, Encoding.UTF8, "application/json");

            try
            {
                var result = await client.PostAsync(siteURL + psRestUrl + "/Projects('" + projectGUID + "')/Draft/Tasks/Add", contents);
                var postResult = result.EnsureSuccessStatusCode();
                if (postResult.IsSuccessStatusCode)
                    isSuccess = true;

                return isSuccess;
            }
            catch (Exception e)
            {
                return isSuccess;
            }

        }

        //requires additional headers MERGE
        public async Task<bool> UpdateTask(string body, string projectGUID, string taskId)
        {

            Boolean isSuccess = false;
            var contents = new StringContent(body, Encoding.UTF8, "application/json");

            try
            {
                var result = await client.PostAsync(siteURL + psRestUrl + "/Projects('" + projectGUID + "')/Draft/Tasks('" + taskId + "')", contents);
                var postResult = result.EnsureSuccessStatusCode();
                if (postResult.IsSuccessStatusCode)
                    isSuccess = true;

                return isSuccess;
            }
            catch (Exception e)
            {
                return isSuccess;
            }

        }

        public async Task<bool> DeleteTask(string body, string projectGUID, string taskId)
        {

            Boolean isSuccess = false;
            var contents = new StringContent(body, Encoding.UTF8, "application/json");

            try
            {
                var result = await client.PostAsync(siteURL + psRestUrl + "/Projects('" + projectGUID + "')/Draft/Tasks('" + taskId + "')/deleteObject()", contents);
                var postResult = result.EnsureSuccessStatusCode();
                if (postResult.IsSuccessStatusCode)
                    isSuccess = true;

                return isSuccess;
            }
            catch (Exception e)
            {
                return isSuccess;
            }

        }

        public async Task<String> GetTimesheetPeriods()
        {

            try
            {
                var result = await client.GetStringAsync(siteURL + psRestUrl + "/timesheetperiods");
                return result;
            }
            catch (Exception e)
            {
                return e.Message;
            }

        }

        public async Task<bool> CreateTimesheet(string body, string periodId)
        {

            Boolean isSuccess = false;
            var contents = new StringContent(body, Encoding.UTF8, "application/json");

            try
            {
                var result = await client.PostAsync(siteURL + psRestUrl + "/TimesheetPeriods('" + periodId + "')/createTimesheet()", contents);
                var postResult = result.EnsureSuccessStatusCode();
                if (postResult.IsSuccessStatusCode)
                    isSuccess = true;

                return isSuccess;
            }
            catch (Exception e)
            {
                return isSuccess;
            }

        }

        public async Task<bool> SubmitTimesheet(string body, string periodId, string comment)
        {

            Boolean isSuccess = false;
            var contents = new StringContent(body, Encoding.UTF8, "application/json");

            try
            {
                var result = await client.PostAsync(siteURL + psRestUrl + "/TimesheetPeriods('" + periodId + "')/Timesheet/submit(" + comment + ")", contents);
                var postResult = result.EnsureSuccessStatusCode();
                if (postResult.IsSuccessStatusCode)
                    isSuccess = true;

                return isSuccess;
            }
            catch (Exception e)
            {
                return isSuccess;
            }

        }

        public async Task<String> GetTimesheetLines(string periodId)
        {

            try
            {
                var result = await client.GetStringAsync(siteURL + psRestUrl + "/TimesheetPeriods('" + periodId + "')/Timesheet/Lines");
                return result;
            }
            catch (Exception e)
            {
                return e.Message;
            }

        }

        public async Task<bool> AddTimesheetLine(string body, string periodId)
        {

            Boolean isSuccess = false;
            var contents = new StringContent(body, Encoding.UTF8, "application/json");

            try
            {
                var result = await client.PostAsync(siteURL + psRestUrl + "/TimesheetPeriods('" + periodId + "')/Timesheet/Lines/Add", contents);
                var postResult = result.EnsureSuccessStatusCode();
                if (postResult.IsSuccessStatusCode)
                    isSuccess = true;

                return isSuccess;
            }
            catch (Exception e)
            {
                return isSuccess;
            }
        }

        public async Task<bool> DeleteTimesheetLine(string body, string periodId, string lineId)
        {

            Boolean isSuccess = false;
            var contents = new StringContent(body, Encoding.UTF8, "application/json");

            try
            {
                var result = await client.PostAsync(siteURL + psRestUrl + "/TimesheetPeriods('" + periodId + "')/Timesheet/Lines('" + lineId + "')/deleteObject()", contents);
                var postResult = result.EnsureSuccessStatusCode();
                if (postResult.IsSuccessStatusCode)
                    isSuccess = true;

                return isSuccess;
            }
            catch (Exception e)
            {
                return isSuccess;
            }

        }

        public async Task<String> GetTimesheetLineWork(string periodId, string lineId)
        {

            try
            {
                var result = await client.GetStringAsync(siteURL + psRestUrl + "/TimesheetPeriods('" + periodId + "')/Timesheet/Lines('" + lineId + "')/Work");
                return result;
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }

        public async Task<bool> AddTimesheetLineWork(string body, string periodId, string lineId)
        {

            Boolean isSuccess = false;
            var contents = new StringContent(body, Encoding.UTF8, "application/json");

            try
            {
                var result = await client.PostAsync(siteURL + psRestUrl + "/TimesheetPeriods('" + periodId + "')/Timesheet/Lines('" + lineId + "')/Work/Add", contents);
                var postResult = result.EnsureSuccessStatusCode();
                if (postResult.IsSuccessStatusCode)
                    isSuccess = true;

                return isSuccess;
            }
            catch (Exception e)
            {
                return isSuccess;
            }
        }

        public async Task<bool> AddAssignmentOnTask(string body, string projectId)
        {

            Boolean isSuccess = false;
            var contents = new StringContent(body, Encoding.UTF8, "application/json");

            try
            {
                var result = await client.PostAsync(siteURL + psRestUrl + "/Projects('" + projectId + "')/Draft/Assignments/Add", contents);
                var postResult = result.EnsureSuccessStatusCode();
                if (postResult.IsSuccessStatusCode)
                    isSuccess = true;

                return isSuccess;
            }
            catch (Exception e)
            {
                return isSuccess;
            }

        }

        public async Task<String> GetEnterpriseResources()
        {

            try
            {
                var result = await client.GetStringAsync(siteURL + psRestUrl + "/EnterpriseResources");
                return result;
            }
            catch (Exception e)
            {
                return e.Message;
            }

        }

        public async Task<bool> AddEnterpriseResource(string body)
        {

            Boolean isSuccess = false;
            var contents = new StringContent(body, Encoding.UTF8, "application/json");

            try
            {
                var result = await client.PostAsync(siteURL + psRestUrl + "/EnterpriseResources/add", contents);
                var postResult = result.EnsureSuccessStatusCode();
                if (postResult.IsSuccessStatusCode)
                    isSuccess = true;

                return isSuccess;
            }
            catch (Exception e)
            {
                return isSuccess;
            }
        }

        public async Task<bool> DeleteEnterpriseResource(string body, string enterpriseResourceId)
        {

            Boolean isSuccess = false;
            var contents = new StringContent(body, Encoding.UTF8, "application/json");

            try
            {
                var result = await client.PostAsync(siteURL + psRestUrl + "/EnterpriseResources('" + enterpriseResourceId + "')/deleteObject()", contents);
                var postResult = result.EnsureSuccessStatusCode();
                if (postResult.IsSuccessStatusCode)
                    isSuccess = true;

                return isSuccess;
            }
            catch (Exception e)
            {
                return isSuccess;
            }

        }

        //requires additional headers MERGE
        public async Task<bool> UpdateEnterpriseResource(string body, string enterpriseResourceId)
        {

            Boolean isSuccess = false;
            var contents = new StringContent(body, Encoding.UTF8, "application/json");

            try
            {
                var result = await client.PostAsync(siteURL + psRestUrl + "/EnterpriseResources('" + enterpriseResourceId + "')", contents);
                var postResult = result.EnsureSuccessStatusCode();
                if (postResult.IsSuccessStatusCode)
                    isSuccess = true;

                return isSuccess;
            }
            catch (Exception e)
            {
                return isSuccess;
            }

        }

    }
}
