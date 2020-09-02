// Copyright (c)2020 Jacobs Data Solutions

// Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License. You may obtain a copy of the
// License at

// http://www.apache.org/licenses/LICENSE-2.0

// Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR
// CONDITIONS OF ANY KIND, either express or implied. See the License for the specific language governing permissions and limitations under the License.
import {
  animate,
  query,
  style,
  transition,
  trigger,
  stagger,
  sequence
} from "@angular/animations";
import { AnimationsService } from "./animations.service";

export const ROUTE_ANIMATIONS_ELEMENTS = "route-animations-elements";

const STEPS_ALL: any[] = [
  query(":enter > *", style({ opacity: 0, position: "fixed" }), {
    optional: true
  }),
  query(":enter ." + ROUTE_ANIMATIONS_ELEMENTS, style({ opacity: 0 }), {
    optional: true
  }),
  sequence([
    query(
      ":leave > *",
      [
        style({ transform: "translateY(0%)", opacity: 1 }),
        animate(
          "0.2s ease-in-out",
          style({ transform: "translateY(-3%)", opacity: 0 })
        ),
        style({ position: "fixed" })
      ],
      { optional: true }
    ),
    query(
      ":enter > *",
      [
        style({
          transform: "translateY(-3%)",
          opacity: 0,
          position: "static"
        }),
        animate(
          "0.5s ease-in-out",
          style({ transform: "translateY(0%)", opacity: 1 })
        )
      ],
      { optional: true }
    )
  ]),
  query(
    ":enter ." + ROUTE_ANIMATIONS_ELEMENTS,
    stagger(75, [
      style({ transform: "translateY(10%)", opacity: 0 }),
      animate(
        "0.5s ease-in-out",
        style({ transform: "translateY(0%)", opacity: 1 })
      )
    ]),
    { optional: true }
  )
];
const STEPS_NONE = [];
const STEPS_PAGE = [STEPS_ALL[0], STEPS_ALL[2]];
const STEPS_ELEMENTS = [STEPS_ALL[1], STEPS_ALL[3]];

export const routeAnimations = trigger("routeAnimations", [
  transition(isRouteAnimationsAll, STEPS_ALL),
  transition(isRouteAnimationsNone, STEPS_NONE),
  transition(isRouteAnimationsPage, STEPS_PAGE),
  transition(isRouteAnimationsElements, STEPS_ELEMENTS)
]);

export function isRouteAnimationsAll() {
  return AnimationsService.isRouteAnimationsType("ALL");
}

export function isRouteAnimationsNone() {
  return AnimationsService.isRouteAnimationsType("NONE");
}

export function isRouteAnimationsPage() {
  return AnimationsService.isRouteAnimationsType("PAGE");
}

export function isRouteAnimationsElements() {
  return AnimationsService.isRouteAnimationsType("ELEMENTS");
}
