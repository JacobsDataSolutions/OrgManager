using JDS.OrgManager.Application.Common.Addresses;
using JDS.OrgManager.Domain.Common.Addresses;

namespace JDS.OrgManager.Application.Common.Mapping
{
    public partial class AddressDbEntityToValueObjectMapper
    {
        public override Address Map(IAddressEntity source) => new Address(source.Address1, source.City, new State(source.State), new ZipCode(source.ZipCode), source.Address2);
    }
}