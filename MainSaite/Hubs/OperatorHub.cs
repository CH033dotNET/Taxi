using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR.Hubs;
using Model.DTO;
using MainSaite.Models;
namespace MainSaite.Hubs
{
	[HubName("OperatorHub")]
	public class OperatorHub : Hub
	{
		static ICollection<SignalRUser> operatorHubUsers = new List<SignalRUser>();


		//deny Client Order
		[HubMethodName("deniedClientOrder")]
		public void DeniedClientOrder(int clientUserId)
		{
			var clientConnectionId = operatorHubUsers.FirstOrDefault(x => x.UserId == clientUserId).ConnectionId;
			Clients.Client(clientConnectionId).deniedClientOrder();
		}


		//delete denied order from new orders table
		[HubMethodName("deleteDeniedClientOrder")]
		public void DeleteDeniedClientOrder(int orderId)
		{
			Clients.Group("Operator").deleteDeniedOrder(orderId);
		}


		//New order from client
		[HubMethodName("sendNewOrderToOperators")]
		public void SendNewOrderToOperators(OrderDTO newOrder)
		{
			Clients.Group("Operator").newOrderFromClient(newOrder);
		}


		//Show modal form 'no free cars'
		[HubMethodName("noFreeCarClientOrder")]
		public void NoFreeCarClientOrder(int clientId)
		{
			var clientConnectionId = operatorHubUsers.FirstOrDefault(x => x.UserId == clientId).ConnectionId;
			Clients.Client(clientConnectionId).noFreeCar();
		}


		//Show modal form 'wait taxi' to client
		[HubMethodName("confirmClientOrder")]
		public void ConfirmClientOrder(string waitingTime, double lat, double lng)
		{
			Clients.Group("Client").waitYourCar(waitingTime, lat, lng);
		}


		//send message from operators to drivers
		[HubMethodName("sendToDrivers")]
		public void SendToDrivers(string message)
		{
			Clients.Group("Driver").showMessage(message); //////////////!!!!!!!!!!!!!!!!!!!!!!!!
		}


		//Send order to driver's table
		[HubMethodName("orderForDrivers")]
		public void OrderForDrivers(OrderDTO order)
		{
			//Clients.Group("Driver").newDriverOrders(order); //////////////!!!!!!!!!!!!!!!!!!!!!!!!
		}


		//send wait order to all operators and delete from newOrdertable
		[HubMethodName("waitingOrderOp")]
		public void WaitingOrderOp(OrderDTO order)
		{
			Clients.Group("Operator").addWaitingOrder(order);
		}


		//remove current order from drivers
		[HubMethodName("removeAwaitOrder")]
		public void RemoveAwaitOrder(int orderId)
		{
			//Clients.Group("Driver").removeAwaitOrders(orderId); //////////////!!!!!!!!!!!!!!!!!!!!!!!!
		}


		//Remove current order from awaiting table  operators
		[HubMethodName("removeAwaitOrders")]
		public void RemoveAwaitOrders(int orderId)
		{
			Clients.Group("Operator").removeAwaitOrders(orderId);
		}


		//remove current order from operators (confirmed by drivers)
		[HubMethodName("removeAwaitOrderFromOperators")]
		public void removeAwaitOrderFromOperators(int orderId)
		{
			Clients.Group("Operator").removeAwaitOrderFromOperators(orderId);
		}


		//send confirmRequest to selected driver
		[HubMethodName("confirmRequest")]
		public void ConfirmRequest(int driverId)
		{
			var driverConnectionId = operatorHubUsers.FirstOrDefault(x => x.UserId == driverId).ConnectionId;
			//Clients.Client(driverConnectionId).confirmDrRequest(); //////////////!!!!!!!!!!!!!!!!!!!!!!!!
		}


		//deny driver request and delete from operator's confirmed table
		[HubMethodName("deniedRequest")]
		public void DeniedRequest(int OrderId)
		{
			Clients.Group("Operator").deleteDrRequest(OrderId);
		}



		[HubMethodName("connectUser")]
		public void ConnectUser(int roleId, int userId)
		{
			string connectionId = Context.ConnectionId;
			int RoleId = roleId;

			var currentUser = new SignalRUser() { ConnectionId = connectionId, RoleId = roleId, UserId = userId };
			if (!operatorHubUsers.Any(x => x.ConnectionId == connectionId))
			{
				if (roleId == 1)
				{
					currentUser.Group = "Driver";
					Groups.Add(Context.ConnectionId, "Driver");
				}
				if (roleId == 2)
				{
					currentUser.Group = "Operator";
					Groups.Add(Context.ConnectionId, "Operator");
				}
				if (roleId == 3)
				{
					currentUser.Group = "Client";
					Groups.Add(Context.ConnectionId, "Client");
				}
				operatorHubUsers.Add(currentUser);

			}
		}

		public override System.Threading.Tasks.Task OnDisconnected(bool stopCalled)
		{
			var item = operatorHubUsers.FirstOrDefault(x => x.ConnectionId == Context.ConnectionId);
			if (item != null)
			{
				operatorHubUsers.Remove(item);
			}

			return base.OnDisconnected(stopCalled);
		}
	}
}