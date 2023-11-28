using FairPlayScheduler.Api.Model;

namespace FairPlayScheduler.Api.Service
{
    public interface ITemplateService
    {
        string RenderResponsibilityDigestEmailTemplate(ResponsibilityByDay responsibilities);
    }
}