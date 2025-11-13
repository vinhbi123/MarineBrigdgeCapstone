using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mediator;
using ShipCapstone.Domain.Models;

namespace ShipCapstone.Domain.Models.ReportProblemCommand;

    public class CreateReportProblemCommand : IRequest<ApiResponse>
    {
        public Guid PortId { get; set; }
        public Guid ShipId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
    }