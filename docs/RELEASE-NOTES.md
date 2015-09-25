# CF Greenhouse 1.0 Release Notes [Draft]
## CF Features Not Yet Supported
1. Implications of not having BOSH support for Windows:
  1. Cannot guarantee binary compatibility with diego bosh releases
  2. No bosh rolling updates for CF or core operating system updates in Windows
2. Windows does not support fair sharing of CPU resources, instead hard allocations are used
3. Currently there is no support for container ssh access from the CF cli
4. ICMP must be explicitly enabled by security groups, the default is to deny ICMP egress
5. Firewall logs are not currently emitted into the CF log pipeline

## Stability and Scalability Expectations
1. Capacity planning for Windows instances of CF varies greatly based upon the overhead of components added by the customer to the instance
2. More detail and guidance for Windows scaling expectations coming soon

## Supported Applications - Known to work
1. [ASP .NET MVC](https://github.com/cloudfoundry-incubator/wats/tree/af669382b4639e7605afc23f1dc8d48d8bfa5dd1/assets/nora/NoraPublished) (12-factor ASP.NET MVC apps compiled against .NET 3.5+ were tested most extensively)
2. [Windows-compiled executables](https://github.com/cloudfoundry-incubator/wats/tree/af669382b4639e7605afc23f1dc8d48d8bfa5dd1/assets/webapp)
3. [Batch scripts (with a manually specified start command)](https://github.com/cloudfoundry-incubator/wats/tree/af669382b4639e7605afc23f1dc8d48d8bfa5dd1/assets/batch-script)

## Applications Not Supported
1. [WCF Applications](http://forums.iis.net/t/1174466.aspx)

## Upgradability
1. Diego is expected to retain backwards compatibility with the cells, which allows for rolling upgrades
2. Greenhouse/.NET will implement cell evacuation prior to the next release to support upgrades
3. Proposed Windows cell upgrade process:
  1. Spin up a new cell
  2. Trigger evacuation on an old cell. The apps from the old cell will be migrated to the new cell
  3. Shut down the old cell when evacuation completes
  4. Repeat until all cells are updated
