using Microsoft.Extensions.Localization;

namespace Core.Infrastructure.Validator
{
    public class StringLocalizerWrapper<T> : IStringLocalizer<T>
    {
        private readonly IStringLocalizer _localizer;

        public StringLocalizerWrapper(IStringLocalizer localizer)
        {
            _localizer = localizer;
        }

        public LocalizedString this[string name] => _localizer[name];

        public LocalizedString this[string name, params object[] arguments] => _localizer[name, arguments];

        public IEnumerable<LocalizedString> GetAllStrings(bool includeParentCultures)
        {
            return _localizer.GetAllStrings(includeParentCultures);
        }
    }
}