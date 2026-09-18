export interface DashboardResponse {
  latestOrders: LatestPurchaseOrder[];
  oldestOrderItems: OldestPurchaseOrderItem[];
  itemQuantities: ItemQuantity[];
}

export interface LatestPurchaseOrder {
  id: number;
  netAmount: number;
  numberOfItems: number;
}

export interface OldestPurchaseOrderItem {
  purchaseOrderId: number;
  itemName: string;
  quantity: number;
}

export interface ItemQuantity {
  itemName: string;
  quantity: number;
}