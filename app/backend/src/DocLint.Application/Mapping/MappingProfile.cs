using AutoMapper;
using DocLint.Application.DTOs;
using DocLint.Domain.Entities;
using DocLint.Domain.Enums;

namespace DocLint.Application.Mapping;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Document, DocumentDto>();

        CreateMap<LintIssue, LintIssueDto>()
            .ForMember(d => d.Rule, o => o.MapFrom(s => new LintRuleDto { Id = s.RuleId, Name = s.RuleName }))
            .ForMember(d => d.Severity, o => o.MapFrom(s => s.Severity.ToString()))
            .ForMember(d => d.Page, o => o.MapFrom(s => s.PageNumber));

        CreateMap<LintReport, LintReportDto>()
            .ForMember(d => d.Document, o => o.MapFrom(s => s.Document))
            .ForMember(d => d.Summary, o => o.MapFrom(s => new SummaryDto
            {
                Status = s.Status.ToString(),
                RuleCount = s.RuleCount,
                IssueCount = s.IssueCount
            }));

        CreateMap<RuleResult, RuleResultDto>()
            .ForMember(d => d.Rule, o => o.MapFrom(s => s.RuleId))
            .ForMember(d => d.Status, o => o.MapFrom(s => s.Status.ToString()));
    }
}
