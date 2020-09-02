// Copyright (c)2020 Jacobs Data Solutions

// Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License. You may obtain a copy of the
// License at

// http://www.apache.org/licenses/LICENSE-2.0

// Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR
// CONDITIONS OF ANY KIND, either express or implied. See the License for the specific language governing permissions and limitations under the License.
import { browser, ExpectedConditions as EC } from "protractor";

import { TodosPage } from "./todos.po";

describe("Todos Page", () => {
  let page: TodosPage;

  beforeEach(() => (page = new TodosPage()));

  it("adds todo", () => {
    page.navigateTo();

    page.getInput().sendKeys("Run e2e tests!");
    page.getAddTodoButton().click();

    browser.wait(EC.presenceOf(page.getResults().get(3)), 5000);

    expect(page.getResults().count()).toBe(4);
    expect(
      page
        .getResults()
        .get(0)
        .getText()
        .then((text) => text.trim())
    ).toBe("Run e2e tests!");
  });
});
