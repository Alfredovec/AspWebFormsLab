using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using BankService.Model;

namespace BankService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IPaymentService" in both code and config file together.
    [ServiceContract]
    public interface IPaymentService
    {
        [OperationContract]
        PaymentStatus PayByVisa(PaymentData data);

        [OperationContract]
        PaymentStatus PayByMasterCard(PaymentData data);

        [OperationContract]
        PaymentStatus Confirm(string token);
    }
}
