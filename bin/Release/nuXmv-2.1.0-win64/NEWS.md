# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.1.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).


## 2.1.0 - 2024-11-29

This is a major release of nuXmv that provides some new features, some
internals improvements and several bug fixes. The major changes include:


### Changed

* Replaced the GNU readline library with the BSD editline library.


### Added

* Add support for the generation of symbolic deterministic automata from
   an Extended Bounded Response LTL specification (LTL-EBR).

* Add support for reqan verification in timed nuxmv.

* Add support for additional nonlinear operators (pow, square root, cos,
  tan, asin, acos, atan).

* Handle input vars in `check_traces_properties`.

* Added support for VMT input (`read_vmt_model` command).


## 2.0.0 - 2019-10-14

This is a major release of nuXmv, containing several new features and
various bug fixes.


### New features

The list of new features within this release are:

* Added support for the specification and verification of symbolic
  infinite-state timed transition systems, allowing for the definition
  of clock variables, the specification of urgent transitions and
  state invariants (typical constructs of timed automata)[^1].

* Extended the representation of traces to support transcendental
  functions and Timed Traces[^1] (i.e. lasso-shaped traces where some
  clock variables may diverge) .

* Extended the parser to allow for the specification of MTL[0,+∞)
  properties, and extended the LTL bounded operators not only to
  contain constants, but also complex expressions over frozen
  variables.

* Extended the model-checking algorithms to support Timed Traces to
  handle verification of LTL/MTL\[0,+∞\] properties of timed
  transition systems[^1].

* Added support for the specification and verification of invariant
  properties of transition systems containing non-linear constraints,
  transcendental functions (sin, cos, tan, log, exp) and the constant
  pi, leveraging on the results discussed and presented in [^2].

* Added support for the declaration of variables of type unbounded
  arrays of Integer/Reals/Bitvectors leveraging on the MathSAT5
  support for unbounded Arrays.

* Integrated the low-level model checking engines that participated to
  the 2016 Hardware Model Checking Competition as replacement of the
  previous ones.

* Integrated the abstract liveness-to-safety algorithm of [^3] for the
  verification of LTL properties of infinite-state systems.

* Extended the nuXmv language to support Parameter Synthesis problems
  for LTL properties, and added corresponding algorithms to compute
  parameter regions[^4].


### Bug fixes

Several bugs were fixed in this version (also those inherited from
NuSMV). Many thanks to those users who reported issues, and helped
improving nuXmv. Here the most critical bug fixes are listed:

* Fixed several corner cases in the SMT encoding, and improved the SMT
  encoding of enumeratives and words to better leverage on the SMT
  capabilities.

* Integrated the latest development version of MathSAT5 that contains
  several bug fixes, optimizations and the support for the analysis of
  non-linear constraints and transcendental functions.

* Integrated several bug-fixes from the development version of MathSAT5.

* Integrated all the bug-fixes from the development version of NuSMV.

* Thoroughly revised the documentation to document the new features and
  to remove typos, ambiguities and repetitions in the grammar, to align
  it with the actual implementation.


### References 

[^1]: A. Cimatti, A. Griggio, E. Magnago, M. Roveri, and S. Tonetta.
"Extending nuXmv with Timed Transition Systems and Timed Temporal
Properties". In I. Dillig and S. Tasiran (Eds.): CAV 2019, LNCS
11561, pp. 1–11, 2019.

[^2]: A. Cimatti, A. Griggio, A. Irfan, M. Roveri, and R. Sebastiani. 
“Incremental linearization for satisfiability and verification
modulo nonlinear arithmetic and transcendental functions". ACM
TOCL, vol. 19, pp. 19:1–19:52, 2018.

[^3]: J. Daniel, A. Cimatti, A. Griggio, S. Tonetta, S. Mover:
"Infinite-State Liveness-to-Safety via Implicit Abstraction and
Well-Founded Relations". CAV (1) 2016: 271-291

[^4]: A. Cimatti, A. Griggio, S. Mover, S. Tonetta: "Parameter synthesis
with IC3". FMCAD 2013: 165-168


## 1.1.1 - 2016-06-01

This is a minor release of nuXmv that provides mainly various bug fixes.


### Bug fixes

The list of minor improvements within this release are:

* Fixed some small bugs related to the Uninterpreted Function feature
  introduced in release 1.1.0.

* Fixed a corner case on the expansion of the bounded past operator H
  (historically).

* Removed some further ambiguities in the user manual, fixed some
  typos missed in previous release, and removed internal debug
  comments.


## 1.1.0 - 2016-05-10

This is a major release of nuXmv, containing new features, and bug
fixes. The documentation has been updated to cover the new features.


### New features

The list of new features within this release are:

* Extended the language to allow for the specification of
  Uninterpreted Functions, and extended the interfaces and all the SMT
  based algorithms to handle designs using Uninterpreted Functions.

* Added the possibility to specify bounded version of the LTL temporal
  operators F (eventually), G (globally), H (historically), and O (once).
  Example: "F \[2, 4\] phi" eventually from 2 to 4 steps phi holds.

* Integrated the low level model checking engines that participated to
  the HWMCC 2015 as replacement of the previous ones.

* Re-enabled commands for performing requirements analysis to check
  for consistency of a set of LTL formulas, for checking possibility
  and assertions in form of LTL formulas w.r.t. a set of LTL
  properties.

* Integrated all the new features and new commands released with
  release 2.6.0 of NuSMV.


### Bug fixes

Several bugs were fixed in this version (also those inherited from
NuSMV). Many thanks to those users who reported issues, and helped
improving nuXmv. Here the most critical bug fixes are listed:

* Fixed several corner cases in the SMT encoding introduced by some of
  the optimizations released in v1.0.1 that prevented the use of SMT
  based algorithms for the verification of designs with infinite
  domain variables or non-bit blasted bit-vectors.

* Integrated several bug-fixes from the development version of MathSAT5.

* Integrated all the bug-fixes from NuSMV 2.6.0 and from its
  development version.

* Revised the documentation to remove typos and ambiguities.


## 1.0.1 - 2014-11-17

This is a minor release of nuXmv that provides some internals
improvements and several bug fixes.


### Changed

The list of minor improvements within this release are:

* Included the low level model checking engines that participated to
  the HWMCC 2014 as replacement of the previous ones.

* Improved the conversion into SMT by avoiding memory and computation
  blow up due to expensive internal transformations to enable for the
  booleanization of finite domain expressions.

* Avoided creation of BDD variables if not needed when verifying a
  design using SMT techniques that often resulted in very expensive
  computations.

* Integrated several optimizations and improvements from the
  development version of NuSMV.

* Improved error message in batch mode for designs with infinite
  domains variables. nuXmv now exits smoothly with an error message
  and suggestions on how to verify the design instead of reporting an
  internal error.


### New features

More that 30 bugs were fixed in this version. Many thanks to those
users who reported issues, and helped improving nuXmv. Here the most
critical bug fixes are listed:

* Fixed several corner cases in the SMT encoding that prevented the
  use of IC3-like algorithms for the verification of designs with
  infinite domain variables or non-bit blasted bit-vectors.

* Fixed a crash within the LTL bounded model checking via SMT
  reported anonymously by a nuXmv user.

* Avoided internal error for not supported operators for models with
  infinite domain variables, providing the user with more informative
  error messages.

* Fixed a bug in the translation from nuXmv to AIGER while encoding
  LTL properties and fairness constraints in the corresponding AIGER
  file.

* Fixed internal reconstruction of the counter example for k-live and
  IC3 based algorithms.

* Integrated several bug-fixes from the development version of NuSMV.
