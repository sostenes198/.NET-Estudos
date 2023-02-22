using DocumentManager.Core.Models;
using MediatR;

namespace DocumentManager.Core.Events;

public record NewDocumentReceived(Document Document) : INotification;