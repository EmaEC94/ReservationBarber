import { Route } from "@angular/router";

import { Page404Component } from "app/authentication/page404/page404.component";
import { LandingPageComponent } from "./windows/landing-page.component";
export const LANDING_PAGE_ROUTE: Route[] = [
  {
    path: "",
    component: LandingPageComponent,
  },
  { path: '**', component: Page404Component },
];
