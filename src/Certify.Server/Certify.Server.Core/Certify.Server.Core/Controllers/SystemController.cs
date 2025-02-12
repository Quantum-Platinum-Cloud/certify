﻿using System.Collections.Generic;
using System.Threading.Tasks;
using Certify.Config.Migration;
using Certify.Management;
using Certify.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace Certify.Service.Controllers
{
    [ApiController]
    [EnableCors()]
    [Route("api/system")]
    public class SystemController : ControllerBase
    {
        private ICertifyManager _certifyManager;

        public SystemController(ICertifyManager manager)
        {
            _certifyManager = manager;
        }

        [AllowAnonymous]
        [HttpGet, Route("appversion")]
        public string GetAppVersion()
        {
            DebugLog();

            return Management.Util.GetAppVersion().ToString();
        }

        [HttpGet, Route("updatecheck")]
        public async Task<UpdateCheck> PerformUpdateCheck()
        {
            DebugLog();

            return await new Management.Util().CheckForUpdates();
        }

        [HttpGet, Route("maintenance")]
        public async Task<string> PerformMaintenanceTasks()
        {
            DebugLog();

            await _certifyManager.PerformCertificateCleanup();
            return "OK";
        }

        [HttpGet, Route("diagnostics")]
        public async Task<List<Models.Config.ActionResult>> PerformServiceDiagnostics()
        {
            DebugLog();

            return await _certifyManager.PerformServiceDiagnostics();
        }

        [HttpPost, Route("migration/export")]
        public async Task<ImportExportPackage> PerformExport(ExportRequest exportRequest)
        {
            return await _certifyManager.PerformExport(exportRequest);
        }

        [HttpPost, Route("migration/import")]
        public async Task<List<ActionStep>> PerformImport(ImportRequest importRequest)
        {
            return await _certifyManager.PerformImport(importRequest);
        }
    }
}
