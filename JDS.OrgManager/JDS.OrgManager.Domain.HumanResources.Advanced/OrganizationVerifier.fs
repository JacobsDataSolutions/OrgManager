// Copyright ©2020 Jacobs Data Solutions

// Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License. You may obtain a copy of the
// License at

// http://www.apache.org/licenses/LICENSE-2.0

// Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR
// CONDITIONS OF ANY KIND, either express or implied. See the License for the specific language governing permissions and limitations under the License.
namespace JDS.OrgManager.Domain.HumanResources.Advanced
open JDS.OrgManager.Domain.HumanResources.Employees
open System.Linq
open System.Collections.Generic

type OrganizationVerifier() =
    let rules = [
        (fun (emp : Employee) -> emp.ValidateAggregate());
        (fun emp -> emp.VerifyEmployeeManagerAndSubordinates());
        (fun emp -> emp.VerifyPtoHoursAreValid());
        (fun emp -> if emp.FirstName = "John" && emp.LastName = "Doe" then raise <| new EmployeeException("Invalid employee name!"))
    ]
    let rec verifyOrgRecursive (emp : Employee) =
        rules
        |> Seq.iter (fun rule -> rule(emp))

        if not <| emp.Subordinates.Any() then
            (1, emp.EmployeeLevel)
        else
            let sCount, sComp = emp.Subordinates |> Seq.map verifyOrgRecursive |> List.ofSeq |> List.unzip |> (fun (c, x) -> (Seq.reduce (+) c, Seq.reduce (+) x))
            (sCount + 1, sComp + emp.EmployeeLevel)

    interface IOrganizationVerifier with
        override this.VerifyOrg(employees : Employee seq) = new List<int * int>(employees |> Seq.map verifyOrgRecursive) :> IReadOnlyList<int * int>