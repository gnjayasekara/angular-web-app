import {
  ChangeDetectorRef,
  Component,
  HostListener,
  inject,
  OnInit,
} from '@angular/core';
import {
  FormBuilder,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';

import { LocationService } from '../../core/services/location.service';
import { Location } from '../../models/location.model';

import { PurchaseBillService } from '../../core/services/purchase-bill.service';
import { Router, RouterLink } from '@angular/router';
import { AuthService } from '../../core/services/auth.service';

interface PurchaseItemRow {
  item: string;
  locationCode: string;
  locationName: string;
  standardCost: number;
  standardPrice: number;
  quantity: number;
  discount: number;
  totalCost: number;
  totalSelling: number;
}

@Component({
  selector: 'app-purchase-bill',
  standalone: true,
  imports: [ReactiveFormsModule, RouterLink],
  templateUrl: './purchase-bill.html',
  styleUrl: './purchase-bill.css',
})
export class PurchaseBillComponent implements OnInit {
  private fb = inject(FormBuilder);
  private locationService = inject(LocationService);
  private purchaseBillService = inject(PurchaseBillService);

  private authService = inject(AuthService);
  private router = inject(Router); 

  private cdr = inject(ChangeDetectorRef);


  locations: Location[] = [];
  isLoadingLocations = false;
  locationError = '';

  isSubmitting = false;
  submitSuccess = '';
  submitError = '';

  items = [
    'Mango',
    'Apple',
    'Banana',
    'Orange',
    'Grapes',
    'Kiwi',
    'Strawberry',
  ];

  filteredItems: string[] = [...this.items];
  showItemSuggestions = false;

  purchaseItems: PurchaseItemRow[] = [];

  purchaseItemForm = this.fb.group({
    item: ['', Validators.required],
    locationCode: ['', Validators.required],
    standardCost: [0, [Validators.required, Validators.min(0)]],
    standardPrice: [0, [Validators.required, Validators.min(0)]],
    quantity: [1, [Validators.required, Validators.min(1)]],
    discount: [ 0, [ Validators.required, Validators.min(0), Validators.max(100), ], ],
    totalCost: [{ value: 0, disabled: true }],
    totalSelling: [{ value: 0, disabled: true }],
  });

  filterItems(): void {
    const itemControl = this.purchaseItemForm.controls.item;

    if (itemControl.hasError('invalidItem')) {
      itemControl.setErrors(null);
    }

    const value = itemControl.value?.toLowerCase() ?? '';

    this.filteredItems = this.items.filter((item) =>
      item.toLowerCase().includes(value)
    );

    this.showItemSuggestions = true;
  }

  selectItem(item: string): void {
    this.purchaseItemForm.controls.item.setValue(item);
    this.showItemSuggestions = false;
  }

  addItem(): void {
    if (this.purchaseItemForm.invalid) {
      this.purchaseItemForm.markAllAsTouched();
      return;
    }

    const formValue = this.purchaseItemForm.getRawValue();

    const enteredItem = formValue.item?.trim() ?? '';

    const isValidItem = this.items.some(
      (item) => item.toLowerCase() === enteredItem.toLowerCase()
    );

    if (!isValidItem) {
      this.purchaseItemForm.controls.item.setErrors({
        invalidItem: true,
      });
      return;
    }

    const selectedLocation = this.locations.find(
      (location) => location.locationCode === formValue.locationCode
    );

    if (!selectedLocation) {
      return;
    }

    const newItem: PurchaseItemRow = {
      item: formValue.item ?? '',
      locationCode: formValue.locationCode ?? '',
      locationName: selectedLocation.locationName,
      standardCost: Number(formValue.standardCost) || 0,
      standardPrice: Number(formValue.standardPrice) || 0,
      quantity: Number(formValue.quantity) || 0,
      discount: Number(formValue.discount) || 0,
      totalCost: Number(formValue.totalCost) || 0,
      totalSelling: Number(formValue.totalSelling) || 0,
    };

    this.purchaseItems.push(newItem);

    this.purchaseItemForm.reset({
      item: '',
      locationCode: '',
      standardCost: 0,
      standardPrice: 0,
      quantity: 1,
      discount: 0,
      totalCost: 0,
      totalSelling: 0,
    });

    this.filteredItems = [...this.items];
    this.showItemSuggestions = false;
  }

  get totalItems(): number {
    return this.purchaseItems.length;
  }

  get totalQuantity(): number {
    return this.purchaseItems.reduce(
      (sum, item) => sum + item.quantity,
      0
    );
  }

  @HostListener('document:click', ['$event'])
    onDocumentClick(event: MouseEvent): void {
      const target = event.target as HTMLElement;

      if (!target.closest('.item-autocomplete')) {
        this.showItemSuggestions = false;
      }
    }

  ngOnInit(): void {
    this.loadLocations();
    this.setupCalculations();
  }


  private loadLocations(): void {
    this.isLoadingLocations = true;
    this.locationError = '';

    this.purchaseItemForm.controls.locationCode.disable();

    this.locationService.getLocations().subscribe({
      next: (locations) => {

        this.locations = locations;
        this.isLoadingLocations = false;

        this.purchaseItemForm.controls.locationCode.enable();
      },
      error: (error) => {
        this.isLoadingLocations = false;
        this.locationError = 'Failed to load locations.';

        this.purchaseItemForm.controls.locationCode.disable();

        console.error('Location loading failed:', error);
      },
    });
  }

  

  submitPurchaseBill(): void {
    if (this.purchaseItems.length === 0) {
      return;
    }

    const request = {
      items: this.purchaseItems.map((item) => ({
        itemName: item.item,
        locationCode: item.locationCode,
        standardCost: item.standardCost,
        standardPrice: item.standardPrice,
        quantity: item.quantity,
        discountPercentage: item.discount,
      })),
    };

    this.isSubmitting = true;
    this.submitSuccess = '';
    this.submitError = '';

    this.purchaseBillService.createPurchaseBill(request).subscribe({
      next: (response) => {
        this.isSubmitting = false;

        this.submitSuccess =
          `Purchase Bill #${response.id} saved successfully.`;

        this.purchaseItems = [];

        this.cdr.detectChanges();

        console.log('Purchase Bill created:', response);
      },

      error: (error) => {
        this.isSubmitting = false;
        this.submitError = 'Failed to save Purchase Bill.';

        this.cdr.detectChanges();

        console.error('Purchase Bill creation failed:', error);
      },
    });
  }

  private setupCalculations(): void {
    this.purchaseItemForm.valueChanges.subscribe(() => {
      this.calculateTotals();
    });
  }

  private calculateTotals(): void {
    const standardCost =
      Number(this.purchaseItemForm.controls.standardCost.value) || 0;

    const standardPrice =
      Number(this.purchaseItemForm.controls.standardPrice.value) || 0;

    const quantity =
      Number(this.purchaseItemForm.controls.quantity.value) || 0;

    const discount =
      Number(this.purchaseItemForm.controls.discount.value) || 0;

    const subtotalCost = standardCost * quantity;

    const discountAmount =
      subtotalCost * (discount / 100);

    const totalCost =
      subtotalCost - discountAmount;

    const totalSelling =
      standardPrice * quantity;

    this.purchaseItemForm.controls.totalCost.setValue(totalCost, {
      emitEvent: false,
    });

    this.purchaseItemForm.controls.totalSelling.setValue(totalSelling, {
      emitEvent: false,
    });
  }
  logout(): void {
    this.authService.logout().subscribe({
      next: () => {
        this.router.navigate(['/login']);
      },
      error: (error) => {
        console.error('Logout failed:', error);

        // Even if the server logout request fails,
        // return the user to the login page.
        this.router.navigate(['/login']);
      },
    });
  }
}