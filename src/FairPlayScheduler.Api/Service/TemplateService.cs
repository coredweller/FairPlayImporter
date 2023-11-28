using AutoMapper;
using FairPlayScheduler.Api.Model;
using FairPlayScheduler.Api.Model.Notifications;
using Nustache.Core;

namespace FairPlayScheduler.Api.Service
{
    public class TemplateService : ITemplateService
    {
        private readonly IMapper _mapper;

        public TemplateService(IMapper mapper)
        {
            _mapper = mapper;
        }

        public string RenderResponsibilityDigestEmailTemplate(ResponsibilityByDay responsibilities)
        {
            var model = _mapper.Map<ResponsibilityByDateEmailModel>(responsibilities);
            string path = Directory.GetCurrentDirectory() + "/Templates/ResponsibilityDigestEmail.template";
            var html = Render.FileToString(path, model);
            return html;
        }
    }
}
