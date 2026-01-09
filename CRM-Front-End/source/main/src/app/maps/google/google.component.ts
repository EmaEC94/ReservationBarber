import { Component, ViewChild } from '@angular/core';
import { CommonModule } from '@angular/common';
import { GoogleMapsModule, MapInfoWindow, MapMarker } from '@angular/google-maps';
import { BreadcrumbComponent } from '../../shared/components/breadcrumb/breadcrumb.component';

@Component({
  selector: 'app-google',
  standalone: true,
  templateUrl: './google.component.html',
  styleUrls: ['./google.component.scss'],
  imports: [
    CommonModule,          // ✅ necesario para *ngFor, *ngIf, etc.
    BreadcrumbComponent,
    GoogleMapsModule
  ]
})
export class GoogleComponent {
  breadscrums = [
    {
      title: 'Google Maps Destiny Barber',
      items: ['Map'],
      active: 'Google Map',
    },
  ];

  locations = [
    {
      name: 'Destiny Barber',
      position: { lat: 10.3335009, lng: -84.4344862 },
      address: 'Plaza San Carlos, Ciudad Quesada',
      link: 'https://www.google.com/maps/place/Destiny+Barber/@10.3335009,-84.4344862'
    }
  ];

  center: google.maps.LatLngLiteral = { lat: 10.3335009, lng: -84.4344862 };
  zoom = 17;

  selected: any = null;

  @ViewChild(MapInfoWindow) infoWindow!: MapInfoWindow;

  openInfo(marker: MapMarker, loc: any) {
    this.selected = loc;
    this.infoWindow.open(marker);
  }
}
