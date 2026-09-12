import {
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
  imports: [ReactiveFormsModule],
  templateUrl: './purchase-bill.html',
  styleUrl: './purchase-bill.css',
})
export class PurchaseBillComponent implements OnInit {
  private fb = inject(FormBuilder);
  private locationService = inject(LocationService);


  locations: Location[] = [];
  isLoadingLocations = false;
  locationError = '';

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
    discount: [0, [Validators.required, Validators.min(0)]],
    totalCost: [{ value: 0, disabled: true }],
    totalSelling: [{ value: 0, disabled: true }],
  });

  filterItems(): void {
    const value =
      this.purchaseItemForm.controls.item.value?.toLowerCase() ?? '';

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

    this.locationService.getLocations().subscribe({
      next: (locations) => {
        this.locations = locations;
        this.isLoadingLocations = false;
      },
      error: (error) => {
        this.isLoadingLocations = false;
        this.locationError = 'Failed to load locations.';
        console.error('Location loading failed:', error);
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

    const totalCost = (standardCost * quantity) - discount;
    const totalSelling = standardPrice * quantity;

    this.purchaseItemForm.controls.totalCost.setValue(totalCost, {
      emitEvent: false,
    });

    this.purchaseItemForm.controls.totalSelling.setValue(totalSelling, {
      emitEvent: false,
    });
  }
}