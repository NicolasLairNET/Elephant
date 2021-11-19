using Elephant.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elephant.Services.ExportService
{
    public interface IExportService
    {
        Task Export(List<TDCTag> tagList);
    }
}
