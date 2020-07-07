using MFS.CommunicationService.Repository;
using OneMFS.SharedResources.Utility;

namespace MFS.CommunicationService.Service
{
    public class MessageService
    {
        private MessageRepository repo;
        public MessageService() {
            repo = new MessageRepository();
        }

        public dynamic SendMessage(MessageModel model)
        {
            return repo.SendSms(model);
        }
    }
}
